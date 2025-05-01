package com.example.devops

import androidx.appcompat.app.AppCompatActivity
import android.os.Bundle
import android.webkit.JavascriptInterface
import android.webkit.WebView
import android.widget.Button
import android.widget.EditText
import android.widget.TableLayout
import android.widget.TableRow
import android.widget.TextView
import kotlinx.coroutines.CoroutineScope
import kotlinx.coroutines.Dispatchers
import kotlinx.coroutines.launch
import retrofit2.Call
import retrofit2.Callback
import retrofit2.Response

class MainActivity : AppCompatActivity() {
    private lateinit var webView: WebView;
    private lateinit var editText: EditText
    private lateinit var buttonGetText: Button
    private var url = "file:///android_asset/index.html";
    private val apiService = RetrofitClient.instance;
    private lateinit var tableLayout: TableLayout

    private var id = 0;

    private lateinit var Users: List<User>;

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        setContentView(R.layout.activity_main)
        webView=findViewById(R.id.wv);
        editText = findViewById(R.id.editText);
        buttonGetText = findViewById(R.id.buttonGetText);
        buttonGetText.setOnClickListener {
            val inputText = editText.text.toString()
            // Используйте значение inputText по вашему усмотрению
            val newUser = User(++id, inputText, 1000);
            //Users = listOf(newUser);
            fillTable(listOf(newUser));
        }

        webView.addJavascriptInterface(WebAppInterface(), "Android")
        webView.settings.javaScriptEnabled = true;
        webView.loadUrl(url);

        tableLayout = findViewById(R.id.tableLayout);

        fetchUsers();
    }

    inner class WebAppInterface {

        /**
         * Этот метод будет вызван из JavaScript при вызове gameOver
         */
        @JavascriptInterface
        fun onGameOver(distanceMeter: Int) {
            // Обработка события gameOver
            println("Game Over!")
            // Дополнительная логика, например, обновление UI
            val inputText = editText.text.toString();
            val floatDist = distanceMeter.toFloat();
            val intDist = floatDist.toInt();
            // Используйте значение inputText по вашему усмотрению
            //Users = listOf(User(++id, inputText, intDist));
            //fillTable(Users);
            fillTable(listOf(User(43, inputText, intDist)));
        }
    }

    fun fillTable(allUsers: List<User> ){
        runOnUiThread{
            for (user in allUsers) {
                val tableRow = TableRow(this)
                tableRow.layoutParams = TableLayout.LayoutParams(
                    TableLayout.LayoutParams.MATCH_PARENT,
                    TableLayout.LayoutParams.WRAP_CONTENT
                )

                val nameTextView = TextView(this)
                nameTextView.text = user.name
                nameTextView.setPadding(8, 8, 8, 8)
                tableRow.addView(nameTextView)

                val scoreTextView = TextView(this)
                scoreTextView.text = user.score.toString()
                scoreTextView.setPadding(8, 8, 8, 8)
                tableRow.addView(scoreTextView)

                tableLayout.addView(tableRow)
            }
            tableLayout.requestLayout();
        }
    }

    fun fetchUsers() {
        val call: Call<List<User>> = apiService.getAllUsers();

        call.enqueue(object : Callback<List<User>> {
            override fun onResponse(call: Call<List<User>>, response: Response<List<User>>) {
                if (response.isSuccessful) {
                    val users = response.body()
                    users?.let {
                        // Обработка списка пользователей
                        println(it);
                        Users = it;
                        fillTable(Users);
                    }
                } else {
                    // Обработка ошибки
                    println("Error: ${response.errorBody()}")
                }
            }

            override fun onFailure(call: Call<List<User>>, t: Throwable) {
                // Обработка исключения
                println("Exception: ${t.message}")
            }
        })
    }

    suspend fun updateUserScore(user: User) {
        CoroutineScope(Dispatchers.IO).launch {
            try {
                val response = apiService.updateScore(user)
                if (response.isSuccessful) {
                    println("Score updated successfully")
                } else {
                    println("Error: ${response.errorBody()}")
                }
            } catch (e: Exception) {
                println("Exception: ${e.message}")
            }
        }
        }


}