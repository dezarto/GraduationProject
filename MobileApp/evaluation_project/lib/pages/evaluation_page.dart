import 'package:flutter/material.dart';


class MyDropDown extends StatelessWidget {
  @override
  Widget build(BuildContext context) {
    return Column(
      mainAxisAlignment: MainAxisAlignment.center,
      children: [
        SizedBox(
          width: double.infinity,
          child: ElevatedButton(
            onPressed: () {
              Navigator.push(
                context,
                MaterialPageRoute(builder: (context) => Group1Page()),
              );
            },
            child: Padding(
              padding: const EdgeInsets.all(20.0),
              child: Text(
                'Group 1',
                style: TextStyle(fontSize: 18),
              ),
            ),
          ),
        ),
        SizedBox(height: 16),
        SizedBox(
          width: double.infinity,
          child: ElevatedButton(
            onPressed: () {
              Navigator.push(
                context,
                MaterialPageRoute(builder: (context) => Group2Page()),
              );
            },
            child: Padding(
              padding: const EdgeInsets.all(20.0),
              child: Text(
                'Group 2',
                style: TextStyle(fontSize: 18),
              ),
            ),
          ),
        ),
        SizedBox(height: 16),
        SizedBox(
          width: double.infinity,
          child: ElevatedButton(
            onPressed: () {
              Navigator.push(
                context,
                MaterialPageRoute(builder: (context) => Group3Page()),
              );
            },
            child: Padding(
              padding: const EdgeInsets.all(20.0),
              child: Text(
                'Group 3',
                style: TextStyle(fontSize: 18),
              ),
            ),
          ),
        ),
        SizedBox(height: 16),
        SizedBox(
          width: double.infinity,
          child: ElevatedButton(
            onPressed: () {
              Navigator.push(
                context,
                MaterialPageRoute(builder: (context) => Group4Page()),
              );
            },
            child: Padding(
              padding: const EdgeInsets.all(20.0),
              child: Text(
                'Group 4',
                style: TextStyle(fontSize: 18),
              ),
            ),
          ),
        ),
      ],
    );
  }
}

class Group1Page extends StatelessWidget {
  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(
        title: Text('Group 1 Grades'),
        backgroundColor: Colors.lightBlueAccent,
      ),
      body: Center(
        child: DataTable(
          columns: const <DataColumn>[
            DataColumn(label: Text('Evaluation')),
            DataColumn(label: Text('Grades')),
            DataColumn(label: Text('Detail')),
          ],
          rows: <DataRow>[
            DataRow(cells: <DataCell>[
              DataCell(Text('Technical Merits')),
              DataCell(Text('15')),
              DataCell(Text('Comment')),
            ]),
            DataRow(cells: <DataCell>[
              DataCell(Text('Project Design and Implementation')),
              DataCell(Text('40')),
              DataCell(Text('Comment')),
            ]),
            DataRow(cells: <DataCell>[
              DataCell(Text('Report')),
              DataCell(Text('15')),
              DataCell(Text('Comment')),
            ]),
            DataRow(cells: <DataCell>[
              DataCell(Text('Presentation')),
              DataCell(Text('10')),
              DataCell(Text('Comment')),
            ]),
            DataRow(cells: <DataCell>[
              DataCell(Text('Demo')),
              DataCell(Text('10')),
              DataCell(Text('Comment')),
            ]),
            DataRow(cells: <DataCell>[
              DataCell(Text('Papers')),
              DataCell(Text('10')),
              DataCell(Text('Comment')),
            ]),
          ],
        ),
      ),
    );
  }
}

class Group2Page extends StatelessWidget {
  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(
        title: Text('Group 2 Grades'),
        backgroundColor: Colors.lightBlueAccent,
      ),
      body: Center(
        child: DataTable(
          columns: const <DataColumn>[
            DataColumn(label: Text('Evaluation')),
            DataColumn(label: Text('Grades')),
            DataColumn(label: Text('Detail')),
          ],
          rows: <DataRow>[
            DataRow(cells: <DataCell>[
              DataCell(Text('Technical Merits')),
              DataCell(Text('15')),
              DataCell(Text('Comment')),
            ]),
            DataRow(cells: <DataCell>[
              DataCell(Text('Project Design and Implementation')),
              DataCell(Text('40')),
              DataCell(Text('Comment')),
            ]),
            DataRow(cells: <DataCell>[
              DataCell(Text('Report')),
              DataCell(Text('15')),
              DataCell(Text('Comment')),
            ]),
            DataRow(cells: <DataCell>[
              DataCell(Text('Presentation')),
              DataCell(Text('10')),
              DataCell(Text('Comment')),
            ]),
            DataRow(cells: <DataCell>[
              DataCell(Text('Demo')),
              DataCell(Text('10')),
              DataCell(Text('Comment')),
            ]),
            DataRow(cells: <DataCell>[
              DataCell(Text('Papers')),
              DataCell(Text('10')),
              DataCell(Text('Comment')),
            ]),
          ],
        ),
      ),
    );
  }
}

class Group3Page extends StatelessWidget {
  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(
        title: Text('Group 3 Grades'),
        backgroundColor: Colors.lightBlueAccent,
      ),
      body: Center(
        child: DataTable(
          columns: const <DataColumn>[
            DataColumn(label: Text('Evaluation')),
            DataColumn(label: Text('Grades')),
            DataColumn(label: Text('Detail')),
          ],
          rows: <DataRow>[
            DataRow(cells: <DataCell>[
              DataCell(Text('Technical Merits')),
              DataCell(Text('15')),
              DataCell(Text('Comment')),
            ]),
            DataRow(cells: <DataCell>[
              DataCell(Text('Project Design and Implementation')),
              DataCell(Text('40')),
              DataCell(Text('Comment')),
            ]),
            DataRow(cells: <DataCell>[
              DataCell(Text('Report')),
              DataCell(Text('15')),
              DataCell(Text('Comment')),
            ]),
            DataRow(cells: <DataCell>[
              DataCell(Text('Presentation')),
              DataCell(Text('10')),
              DataCell(Text('Comment')),
            ]),
            DataRow(cells: <DataCell>[
              DataCell(Text('Demo')),
              DataCell(Text('10')),
              DataCell(Text('Comment')),
            ]),
            DataRow(cells: <DataCell>[
              DataCell(Text('Papers')),
              DataCell(Text('10')),
              DataCell(Text('Comment')),
            ]),
          ],
        ),
      ),
    );
  }
}

class Group4Page extends StatelessWidget {
  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(
        title: Text('Group 4 Grades'),
        backgroundColor: Colors.lightBlueAccent,
      ),
      body: Center(
        child: DataTable(
          columns: const <DataColumn>[
            DataColumn(label: Text('Evaluation')),
            DataColumn(label: Text('Grades')),
            DataColumn(label: Text('Detail')),
          ],
          rows: <DataRow>[
            DataRow(cells: <DataCell>[
              DataCell(Text('Technical Merits')),
              DataCell(Text('15')),
              DataCell(Text('Comment')),
            ]),
            DataRow(cells: <DataCell>[
              DataCell(Text('Project Design and Implementation')),
              DataCell(Text('40')),
              DataCell(Text('Comment')),
            ]),
            DataRow(cells: <DataCell>[
              DataCell(Text('Report')),
              DataCell(Text('15')),
              DataCell(Text('Comment')),
            ]),
            DataRow(cells: <DataCell>[
              DataCell(Text('Presentation')),
              DataCell(Text('10')),
              DataCell(Text('Comment')),
            ]),
            DataRow(cells: <DataCell>[
              DataCell(Text('Demo')),
              DataCell(Text('10')),
              DataCell(Text('Comment')),
            ]),
            DataRow(cells: <DataCell>[
              DataCell(Text('Papers')),
              DataCell(Text('10')),
              DataCell(Text('Comment')),
            ]),
          ],
        ),
      ),
    );
  }
}
