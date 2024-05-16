import 'package:flutter/material.dart';
import 'package:cloud_firestore/cloud_firestore.dart';

class GroupPage extends StatelessWidget {
  final String groupName;
  final Map<String, dynamic>? groupData;

  GroupPage(this.groupName, this.groupData);

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(
        centerTitle: true,
        title: Text(
          '$groupName Grades',
          style: TextStyle(
            color: Colors.white,
            fontSize: 40.0,
            fontWeight: FontWeight.bold,
          ),
        ),
        backgroundColor: Colors.lightBlueAccent,
      ),
      body: Center(
        child: groupData != null
            ? SingleChildScrollView(
          child: Column(
            mainAxisAlignment: MainAxisAlignment.center,
            children: [
              DataTable(
                columns: const <DataColumn>[
                  DataColumn(label: Text('Evaluation')),
                  DataColumn(label: Text('Grades')),
                ],
                rows: [
                  DataRow(
                    cells: [
                      DataCell(Text('Technical Merits')),
                      DataCell(Text('${groupData!['Technical Merits']}')),
                    ],
                  ),
                  DataRow(
                    cells: [
                      DataCell(Text('Project Design and Implementation')),
                      DataCell(Text('${groupData!['Project Design and Implementation']}')),
                    ],
                  ),
                  DataRow(
                    cells: [
                      DataCell(Text('Report')),
                      DataCell(Text('${groupData!['Report']}')),
                    ],
                  ),
                  DataRow(
                    cells: [
                      DataCell(Text('Presentation')),
                      DataCell(Text('${groupData!['Presentation']}')),
                    ],
                  ),
                  DataRow(
                    cells: [
                      DataCell(Text('Demo')),
                      DataCell(Text('${groupData!['Demo']}')),
                    ],
                  ),
                  DataRow(
                    cells: [
                      DataCell(Text('Papers')),
                      DataCell(Text('${groupData!['Papers']}')),
                    ],
                  ),
                ],
              ),
              SizedBox(height: 20),
              Row(
                mainAxisAlignment: MainAxisAlignment.start,
                children: [
                  Text(
                    'Comment: ',
                    style: TextStyle(fontSize: 16, fontWeight: FontWeight.bold),
                  ),
                  Text(
                    '${groupData!['Comment']}',
                    style: TextStyle(fontSize: 16),
                  ),
                ],
              ),
            ],
          ),
        )
            : CircularProgressIndicator(),
      ),
    );
  }
}

class MyDropDown extends StatelessWidget {
  final FirebaseFirestore _firestore = FirebaseFirestore.instance;

  @override
  Widget build(BuildContext context) {
    return StreamBuilder<QuerySnapshot>(
      stream: _firestore.collection('results').snapshots(),
      builder: (context, snapshot) {
        if (!snapshot.hasData) {
          return Center(
            child: CircularProgressIndicator(),
          );
        }
        final List<DocumentSnapshot> documents = snapshot.data!.docs;
        return Scaffold(
          appBar: AppBar(
            centerTitle: true,
            title: Text(
              'Select Group',
              style: TextStyle(
                color: Colors.white,
                fontSize: 40.0,
                fontWeight: FontWeight.bold,
              ),
            ),
            backgroundColor: Colors.lightBlueAccent,
          ),
          body: SingleChildScrollView( // SingleChildScrollView ekledik
            child: Column(
              mainAxisAlignment: MainAxisAlignment.center,
              children: documents.map((doc) {
                final groupName = doc.id;
                return Padding(
                  padding: const EdgeInsets.symmetric(vertical: 10.0),
                  child: SizedBox(
                    width: double.infinity,
                    child: ElevatedButton(
                      onPressed: () async {
                        try {
                          // Firestore'dan veri çekme işlemi
                          DocumentSnapshot documentSnapshot = await _firestore.collection('results').doc(groupName).get();

                          if (documentSnapshot.exists) {
                            Map<String, dynamic>? groupData = documentSnapshot.data() as Map<String, dynamic>?;
                            Navigator.push(
                              context,
                              MaterialPageRoute(
                                builder: (context) => GroupPage(groupName, groupData),
                              ),
                            );
                          } else {
                            ScaffoldMessenger.of(context).showSnackBar(
                              SnackBar(
                                content: Text('Group data for $groupName not found!'),
                              ),
                            );
                          }
                        } catch (e) {
                          print('Error getting group data: $e');
                          ScaffoldMessenger.of(context).showSnackBar(
                            SnackBar(
                              content: Text('An error occurred while getting group data!'),
                            ),
                          );
                        }
                      },
                      style: ButtonStyle(
                        backgroundColor: MaterialStateProperty.all<Color>(Colors.lightBlueAccent),
                      ),
                      child: Padding(
                        padding: const EdgeInsets.all(20.0),
                        child: Text(
                          groupName.toUpperCase(),
                          style: TextStyle(fontSize: 25, color: Colors.white),
                        ),
                      ),
                    ),
                  ),
                );
              }).toList(),
            ),
          ),
        );
      },
    );
  }
}


void main() {
  runApp(MaterialApp(
    home: MyDropDown(),
  ));
}
