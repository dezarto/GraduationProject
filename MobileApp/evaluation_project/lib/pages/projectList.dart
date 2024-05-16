import 'package:evaluation_project/service/UserAuthService.dart';
import 'package:flutter/material.dart';
import 'package:cloud_firestore/cloud_firestore.dart';
import 'package:evaluation_project/pages/EvaluationPage.dart';

class ProjectList extends StatefulWidget {
  const ProjectList({Key? key}) : super(key: key);

  @override
  State<ProjectList> createState() => _ProjectListState();
}

class _ProjectListState extends State<ProjectList> {
  late Stream<QuerySnapshot> _projectsStream;

  @override
  void initState() {
    super.initState();
    _projectsStream = FirebaseFirestore.instance.collection('project-list').snapshots();
    String? username = UsernameService.usernameValue;
    if (username != null) {
      print("Kullanıcı adı: $username");
    } else {
      print("Kullanıcı adı henüz ayarlanmadı.");
    }
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
        body: StreamBuilder(
          stream: _projectsStream,
          builder: (BuildContext context, AsyncSnapshot<QuerySnapshot> snapshot) {
            if (snapshot.connectionState == ConnectionState.waiting) {
              return Center(child: CircularProgressIndicator());
            }
            if (snapshot.hasError) {
              return Center(child: Text('Something went wrong'));
            }

            // Separate completed and not completed projects
            List<DocumentSnapshot> completedProjects = [];
            List<DocumentSnapshot> notCompletedProjects = [];
            snapshot.data!.docs.forEach((doc) {
              if (doc['status'] == 'Completed') {
                completedProjects.add(doc);
              } else {
                notCompletedProjects.add(doc);
              }
            });

            // Sort completed projects by date if needed
            completedProjects.sort((a, b) => a['date'].compareTo(b['date']));

            return ListView(
              children: [
                ...completedProjects.map((doc) => _buildProjectItem(doc)),
                ...notCompletedProjects.map((doc) => _buildProjectItem(doc)),
              ],
            );
          },
        ),
      ),
    );
  }

  Widget _buildProjectItem(DocumentSnapshot project) {
    String projectName = project['projectName'] ?? '';
    String projectOwner = project['projectOwner'] ?? '';
    String projectDate = project['date'] ?? ''; // Firestore'dan gelen tarih
    bool isCompleted = project['status'] == 'Completed';

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
              Text('Date: $projectDate'), // Firestore'dan gelen tarih
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
  }
}
