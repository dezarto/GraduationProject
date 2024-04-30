import 'package:evaluation_project/pages/projectList.dart';
import 'package:flutter/material.dart';
import 'package:flutter_svg/flutter_svg.dart';
import 'package:evaluation_project/components/rounded_input_field.dart';
import 'package:evaluation_project/components/rounded_button.dart';


import 'dart:convert';
import 'package:http/http.dart' as http;

class LoginScreen extends StatelessWidget {
  //burası
  final TextEditingController usernameController = TextEditingController();
  final TextEditingController passwordController = TextEditingController();

  LoginScreen({Key? key});

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
              const SizedBox(
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
                    margin: const EdgeInsets.all(5),
                    padding: const EdgeInsets.all(20),
                    height: 300,
                    width: 350,
                    decoration: BoxDecoration(
                      color: const Color(0xffF3F3F5),
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
                          controller: usernameController, // Kontrolcüyü bağlayın
                        ),
                        RoundedInputField(
                          isPassword: true,
                          hintText: "Password",
                          icon: Icons.lock,
                          onChange: (value) {
                            // Şifre değiştiğinde yapılacak işlemler buraya
                          },
                          controller: passwordController, // Kontrolcüyü bağlayın
                        ),
                        RoundedButton(
                          text: "LOGIN",
                          press: () async {
                            var backResponse = await loginUser(
                              usernameController.text,
                              passwordController.text,
                            );

                            if (backResponse.statusCode == 200) {
                              print("backResponse başarılı");
                              Navigator.of(context).push(MaterialPageRoute(builder: (context) => ProjectList()));
                            } else {
                              print('backResponse Giriş başarısız. Hata kodu: ${backResponse.statusCode}');
                            }
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
                "lib/"
                    "assets/images/down.svg",
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

//burası

Future<http.Response> loginUser(String username, String password) async {
  var url = Uri.parse('https://10.0.2.2:7225/api/Login/login');
  var response = await http.post(
    url,
    headers: {'Content-Type': 'application/json'},
    body: jsonEncode({'Username': username, 'Password': password}),
  );
  print("Satır 127");
  print(response.statusCode);
  print(response.body);

  if (response.statusCode == 200) {
    print("giriş başarılı");
    var responseData = jsonDecode(response.body);
    print(responseData);
    var token = responseData['token'];
    // Token ile kullanıcıyı yetkilendirme işlemleri burada yapılır

  } else {
    print('Giriş başarısız. Hata kodu: ${response.statusCode}');
  }

  return response;
}

