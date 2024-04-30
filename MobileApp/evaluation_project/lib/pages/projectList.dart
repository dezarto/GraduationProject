import 'package:flutter/material.dart';
import 'dart:math';
import 'package:evaluation_project/pages/EvaluationPage.dart';
class ProjectList extends StatefulWidget {
  const ProjectList({super.key});

  @override
  State<ProjectList> createState() => _ProjectListState();
}

class _ProjectListState extends State<ProjectList> {
  final int itemCount = 4;
  final List<String> projects = [
    'Project A',
    'Project B',
    'Project C',
    'Project D'
  ];

  String getRandomDate() {
    Random random = Random();
    int year = 2021;
    int month = random.nextInt(12) + 1;
    int day = random.nextInt(28) + 1;
    return '$month/$day/$year';
  }

  @override
  Widget build(BuildContext context) {
    return MaterialApp(
      debugShowCheckedModeBanner: false,
      home: Scaffold(
        backgroundColor: Colors.white,
        appBar: AppBar(
          backgroundColor: Colors.white,
          title: const Text('Projects'),
          actions: [
            IconButton(
              icon: const Icon(Icons.search),
              onPressed: () {},
            ),
          ],
        ),
        body: ListView.builder(
          itemCount: itemCount,
          itemBuilder: (context, index) {
            String projectName = projects[index % projects.length];
            String projectOwner = 'User $index';
            String randomDate = getRandomDate();
            bool isCompleted = index < 2;

            return Container(
              margin: const EdgeInsets.all(8.0),
              padding: const EdgeInsets.all(16.0),
              decoration: BoxDecoration(
                border: Border.all(
                  color: Colors.grey,
                  width: 1.0,
                ),
              ),
              child: Row(
                crossAxisAlignment: CrossAxisAlignment.start,
                children: [
                  Image.asset('lib/assets/images/user.png'),
                  const SizedBox(width: 16.0),
                  Expanded(
                    child: Column(
                      crossAxisAlignment: CrossAxisAlignment.start,
                      children: [
                        Text('Project Name: $projectName'),
                        Text('Project Owner: $projectOwner'),
                        Text(
                          'Status: ${isCompleted ? 'Completed' : 'Not Completed'}',
                          style: TextStyle(
                            fontWeight: FontWeight.bold,
                            color: isCompleted ? Colors.green : Colors.red,
                          ),
                        ),
                      ],
                    ),
                  ),
                  const SizedBox(width: 16.0),
                  Column(
                    crossAxisAlignment: CrossAxisAlignment.end,
                    children: [
                      Text('Date: $randomDate'),
                      const SizedBox(height: 8.0),
                      ElevatedButton(
                        onPressed: () {
                          Navigator.push(
                            context,
                            MaterialPageRoute(builder: (context) => EvaluationPage()),
                          );

                        },
                        child: const Text('Evaluate'),
                      ),
                    ],
                  ),
                ],
              ),
            );
          },
        ),
      ),
    );
  }
}
