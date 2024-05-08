import 'package:flutter/material.dart';
import 'package:flutter_svg/flutter_svg.dart';
import 'package:evaluation_project/components/rounded_input_field.dart';
import 'package:evaluation_project/components/rounded_button.dart';

import 'dart:convert';
import 'package:http/http.dart' as http;

class LoginScreen extends StatelessWidget {
  final TextEditingController username = TextEditingController();
  final TextEditingController password = TextEditingController();

  LoginScreen({Key? key});

  Future<void> loginUser(String username, String password, BuildContext context) async {
    var url = Uri.parse('https://b8ba-92-45-86-194.ngrok-free.app');

    var response = await http.post(
      url,
      body: {
        'Username': username,
        'Password': password,
      },
    );

    if (response.statusCode == 200) {
      var responseData = jsonDecode(response.body);
      var message = responseData['message'];
      var usernameResponse = responseData['usernameResponse'];
      var usernameValue = usernameResponse['username'];

      print(message); // "Login successful"
      print("Oturum açan kullanıcı: $usernameValue");

      Navigator.push(
        context,
        MaterialPageRoute(builder: (context) => Yonlendirilen()),
      );
    } else {
      print('Giriş başarısız. Hata kodu: ${response.statusCode}');
    }
  }


  @override
  Widget build(BuildContext context) {
    return Scaffold(
      resizeToAvoidBottomInset: false,
      body: Stack(
        children: [
          Positioned(
            top: 0,
            child: Opacity(
              opacity: 0.7,
              child: SvgPicture.asset(
                "lib/assets/images/up.svg",
                height: 200,
                width: 100,
              ),
            ),
          ),
          Column(
            children: [
              SizedBox(
                height: 150,
              ),
              Image.asset(
                "lib/assets/images/middd.png",
                height: 80,
              ),
              Row(
                mainAxisAlignment: MainAxisAlignment.center,
                children: [
                  Container(
                    margin: EdgeInsets.all(5),
                    padding: EdgeInsets.all(20),
                    height: 300,
                    width: 350,
                    decoration: BoxDecoration(
                      color: Color(0xffF3F3F5),
                      borderRadius: BorderRadius.circular(20),
                    ),
                    child: Column(
                      children: [
                        RoundedInputField(
                          isPassword: false,
                          hintText: "Username",
                          icon: Icons.account_circle,
                          onChange: (value) {
                            // Kullanıcı adı değiştiğinde yapılacak işlemler buraya
                          },
                          controller: username,
                        ),
                        RoundedInputField(
                          isPassword: true,
                          hintText: "Password",
                          icon: Icons.lock,
                          onChange: (value) {
                            // Şifre değiştiğinde yapılacak işlemler buraya
                          },
                          controller: password,
                        ),
                        RoundedButton(
                          text: "LOGIN",
                          press: () {
                            loginUser(
                              username.text,
                              password.text,
                              context,
                            );
                          },
                          color: Colors.grey,
                        ),
                      ],
                    ),
                  ),
                ],
              ),
            ],
          ),
          Positioned(
            bottom: 0,
            child: Opacity(
              opacity: 0.3,
              child: SvgPicture.asset(
                "lib/assets/images/down.svg",
                height: 130,
                width: 100,
              ),
            ),
          ),
        ],
      ),
    );
  }
}

class Yonlendirilen extends StatelessWidget {
  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(
        title: Text('Yonlendirilen Ekranı'),
      ),
      body: Center(
        child: Text('Başarılı bir şekilde giriş yaptınız!'),
      ),
    );
  }
}