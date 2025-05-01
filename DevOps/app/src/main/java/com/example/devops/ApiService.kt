package com.example.devops

import retrofit2.Call
import retrofit2.Response
import retrofit2.http.Body
import retrofit2.http.GET
import retrofit2.http.POST
import retrofit2.http.Path

interface ApiService {
    @POST("updateScore")
    suspend fun updateScore(@Body user: User): Response<Void>

    @GET("users")
    fun getAllUsers(): Call<List<User>>
}
