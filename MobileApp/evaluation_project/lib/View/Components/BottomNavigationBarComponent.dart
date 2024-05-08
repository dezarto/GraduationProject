import 'package:flutter/material.dart';
import 'package:evaluation_project/pages/evaluation_page.dart';

class BottomNavigationBarComponent extends StatelessWidget {
  @override
  Widget build(BuildContext context) {
    return BottomAppBar(
      child: Row(
        mainAxisAlignment: MainAxisAlignment.spaceAround,
        children: <Widget>[
          IconButton(
            icon: const Icon(Icons.home),
            onPressed: () {
              Navigator.push(
                context,
                MaterialPageRoute(builder: (context) => MyDropDown()),
              );
            },
          ),
        ],
      ),
    );
  }
}
