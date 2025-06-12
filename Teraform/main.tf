terraform {
  required_providers {
    yandex = {
      source = "yandex-cloud/yandex"
    }
  }
  required_version = ">= 0.12"
}

provider "yandex" {
  token     = "y0__xCq6vn4AxjB3RMgwuDVkBM9uP55HhDELOMlBD1M0OxH7_boQA"
  cloud_id  = "b1gnbt5ikmghauv53uom"
  folder_id = "b1gvi1bb4cvmv01crtda"
  zone      = "ru-central1-b"
}

resource "yandex_compute_disk" "boot-disk-2" {
  name     = "boot-disk-3"
  type     = "network-ssd"
  zone     = "ru-central1-b"
  size     = "35"
  image_id = "fd8aus3bfglr6dg9hsbk"  # Ubuntu 22.04 LTS
}

data "yandex_vpc_subnet" "default_subnet" {
  name = "default-ru-central1-b"
}

resource "yandex_compute_instance" "vm-2" {
  name = "k8s-server"

  resources {
    cores  = 4
    memory = 4
  }

  boot_disk {
    disk_id = yandex_compute_disk.boot-disk-2.id
  }

  network_interface {
    subnet_id = data.yandex_vpc_subnet.default_subnet.id
    nat       = true  # Публичный IP
  }

  metadata = {
    ssh-keys = "ubuntu:${file("~/.ssh/id_ed25519.pub")}"
  }

  connection {
    type        = "ssh"
    user        = "ubuntu"
    private_key = file("~/.ssh/id_ed25519")
    host        = self.network_interface.0.nat_ip_address
  }

  # Установка Docker (если ещё не установлен)
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
    ]
  }

  # Установка kubelet, kubeadm, kubectl
  provisioner "remote-exec" {
    inline = [
      # Отключаем swap (обязательно для Kubernetes)
      "sudo swapoff -a",
      "sudo sed -i '/ swap / s/^/#/' /etc/fstab",

      # Устанавливаем компоненты Kubernetes
      "sudo apt-get update",
      "sudo apt-get install -y apt-transport-https ca-certificates curl",
      "curl -fsSL https://packages.cloud.google.com/apt/doc/apt-key.gpg | sudo gpg --dearmor -o /etc/apt/keyrings/kubernetes-archive-keyring.gpg",
      "echo \"deb [signed-by=/etc/apt/keyrings/kubernetes-archive-keyring.gpg] https://apt.kubernetes.io/ kubernetes-xenial main\" | sudo tee /etc/apt/sources.list.d/kubernetes.list > /dev/null",
      "sudo apt-get update",
      "sudo apt-get install -y kubelet kubeadm kubectl",
      "sudo apt-mark hold kubelet kubeadm kubectl",  # Фиксируем версии

      # Инициализируем кластер (если нужно)
      "sudo kubeadm init --pod-network-cidr=10.244.0.0/16 --ignore-preflight-errors=all || echo 'Kubernetes уже инициализирован'",
      
      # Настраиваем kubectl для текущего пользователя
      "mkdir -p $HOME/.kube",
      "sudo cp -i /etc/kubernetes/admin.conf $HOME/.kube/config",
      "sudo chown $(id -u):$(id -g) $HOME/.kube/config",

      # Устанавливаем сетевой плагин (Flannel)
      "kubectl apply -f https://github.com/flannel-io/flannel/releases/latest/download/kube-flannel.yml",
    ]
  }
}

# Выводим публичный IP сервера
output "vm_public_ip" {
  value = yandex_compute_instance.vm-2.network_interface.0.nat_ip_address
}