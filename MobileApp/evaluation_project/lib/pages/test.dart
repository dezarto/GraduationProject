import 'package:cloud_firestore/cloud_firestore.dart';
import 'package:flutter/material.dart';
import 'package:intl/intl.dart';
class Home extends StatefulWidget {
  const Home({super.key});

  @override
  State<Home> createState() => _HomeState();
}

class _HomeState extends State<Home> {
  final _firestore = FirebaseFirestore.instance;

  @override
  Widget build(BuildContext context) {
    CollectionReference evaluationProjectRef =
    _firestore.collection("evaluation-project");
    DocumentReference professorRef = evaluationProjectRef.doc("Professor");
    CollectionReference academicianRef = professorRef.collection("Academician");
    DocumentReference academicianRef_1 = academicianRef.doc("Academician1");
    String formattedDate = DateFormat('dd-MM-yyyy – kk:mm').format(DateTime.now());
    return Scaffold(
      backgroundColor: Colors.grey,
      appBar: AppBar(
        title: const Text("Home"),
        backgroundColor: Theme.of(context).colorScheme.inversePrimary,
      ),
      body: Center(
        child: Container(
          child: Column(
            children: [
              Text("${professorRef.get()}"),
              ElevatedButton(
                onPressed: () async {
                  // var response = await evaluationProjectRef.get();
                  //var list = response.docs;

                  //var response = await professorRef.get();
                  //var veri = response.data();

                  var response = await academicianRef_1.get();
                  var veri = response.data() as Map<String, dynamic>?;

                  //for (var document in veri) {
                  //  var id = document.id;
                  //  print("Belge ID: $id");
                  // }

                  if (veri != null) {
                    print(veri["Email"]);
                  } else {
                    print("Veri bulunamadı veya tip uyumsuzluğu.");
                  }
                },
                child: Text("get data"),
              ),
              Container(
                child: Text(
                    "${formattedDate}"),
              ),
            ],
          ),
        ),
      ),
    );
  }
}
