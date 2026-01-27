import 'package:flutter/material.dart';
import 'package:provider/provider.dart';
import '../../providers/auth_provider.dart';
import '../auth/login_screen.dart';

class HomeScreen extends StatelessWidget {
  const HomeScreen({super.key});

  @override
  Widget build(BuildContext context) {
    final user = Provider.of<AuthProvider>(context).user;

    return Scaffold(
      appBar: AppBar(
        title: const Text("Trang Chủ"),
        actions: [
          IconButton(
            icon: const Icon(Icons.logout),
            onPressed: () {
              // Đăng xuất và quay về Login
              Provider.of<AuthProvider>(context, listen: false).logout();
              Navigator.pushReplacement(
                context, 
                MaterialPageRoute(builder: (_) => const LoginScreen())
              );
            },
          )
        ],
      ),
      body: Center(
        child: Column(
          mainAxisAlignment: MainAxisAlignment.center,
          children: [
            Text("Xin chào, ${user?.fullName ?? 'Bạn'}!", style: const TextStyle(fontSize: 20)),
            const SizedBox(height: 10),
            Text("Số dư ví: ${user?.walletBalance} đ", style: const TextStyle(fontSize: 24, fontWeight: FontWeight.bold, color: Colors.green)),
            const SizedBox(height: 10),
            Text("Hạng: ${user?.tier}", style: const TextStyle(color: Colors.orange)),
          ],
        ),
      ),
    );
  }
}