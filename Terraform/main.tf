terraform {
  required_providers {
    yandex = {
      source = "yandex-cloud/yandex"
    }
  }
  required_version = ">= 0.12"
}

provider "yandex" {
  token     = "y0__xDhmPzWARjB3RMguoT0kBNDP55OJk8HaG6h7poiF2nJyNI6RA"
  cloud_id  = "b1gcrgvduhipovboqgc0"
  folder_id = "b1gdvic6l0c0tce4fov6"
  zone      = "ru-central1-b"
}

resource "yandex_compute_disk" "boot-disk-2" {
  name     = "boot-disk-2"
  type     = "network-ssd"
  zone     = "ru-central1-b"
  size     = "10"
  image_id = "fd8aus3bfglr6dg9hsbk"
}

data "yandex_vpc_subnet" "default_subnet" {
  name = "default-ru-central1-b"
}

resource "yandex_compute_instance" "vm-2" {
  name = "terraform2"

  resources {
    cores  = 2
    memory = 4
  }

  boot_disk {
    disk_id = yandex_compute_disk.boot-disk-2.id
  }

  network_interface {
    subnet_id = data.yandex_vpc_subnet.default_subnet.id
    nat       = true
  }

  metadata = {
    ssh-keys = "ubuntu:${file("~/.ssh/id_ed25519.pub")}"
  }

  # Добавляем подключение для provisioner
  connection {
    type        = "ssh"
    user        = "ubuntu"  # Пользователь по умолчанию для Ubuntu
    private_key = file("~/.ssh/id_ed25519")  # Соответствующий приватный ключ
    host        = self.network_interface.0.nat_ip_address  # Публичный IP
  }

  # Provisioner для установки Docker
  provisioner "remote-exec" {
    inline = [
      "sudo apt-get update",
      "sudo apt-get install -y apt-transport-https ca-certificates curl software-properties-common",
      "curl -fsSL https://download.docker.com/linux/ubuntu/gpg | sudo gpg --dearmor -o /usr/share/keyrings/docker-archive-keyring.gpg",
      "echo \"deb [arch=$(dpkg --print-architecture) signed-by=/usr/share/keyrings/docker-archive-keyring.gpg] https://download.docker.com/linux/ubuntu $(lsb_release -cs) stable\" | sudo tee /etc/apt/sources.list.d/docker.list > /dev/null",
      "sudo apt-get update",
      "sudo apt-get install -y docker-ce docker-ce-cli containerd.io",
      "sudo systemctl enable docker",
      "sudo systemctl start docker",
      "sudo usermod -aG docker ubuntu",
      "echo 'Docker installed successfully!'"
    ]
  }
}

resource "yandex_vpc_network" "network-1" {
  name = "network1"
}

resource "yandex_vpc_subnet" "subnet-1" {
  name           = "subnet1"
  zone           = "ru-central1-d"
  network_id     = yandex_vpc_network.network-1.id
  v4_cidr_blocks = ["192.168.10.0/24"]
}

output "internal_ip_address_vm_2" {
  value = yandex_compute_instance.vm-2.network_interface.0.ip_address
}

output "external_ip_address_vm_2" {
  value = yandex_compute_instance.vm-2.network_interface.0.nat_ip_address
}
resource "yandex_kubernetes_cluster" "k8s-cluster" {
  name        = "k8s-lab3"
  network_id  = yandex_vpc_network.network-1.id
  release_channel = "REGULAR"
  master {
    version = "1.29"
    public_ip = true
    zonal {
      zone      = "ru-central1-d"
      subnet_id = yandex_vpc_subnet.subnet-1.id
    }
  }
  service_account_id      = "ajeb5guordceis9joibo"
  node_service_account_id = "ajeb5guordceis9joibo"
}

resource "yandex_kubernetes_node_group" "k8s-nodes" {
  cluster_id = yandex_kubernetes_cluster.k8s-cluster.id
  name       = "k8s-nodes"
  instance_template {
    platform_id = "standard-v2"
    resources {
      cores  = 2
      memory = 4
    }
    boot_disk {
      type = "network-ssd"
      size = 64
    }
    network_interface {
      subnet_ids = [yandex_vpc_subnet.subnet-1.id]
      nat        = true
    }
  }
  scale_policy {
    fixed_scale {
      size = 2
    }
  }
}