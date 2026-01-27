import 'package:flutter/material.dart';
import 'package:provider/provider.dart';
import '../../providers/auth_provider.dart';
import '../home/home_screen.dart'; // Chúng ta sẽ tạo file này ở bước sau

class LoginScreen extends StatefulWidget {
  const LoginScreen({super.key});

  @override
  State<LoginScreen> createState() => _LoginScreenState();
}

class _LoginScreenState extends State<LoginScreen> {
  final _emailController = TextEditingController(text: "member1@pcm.com"); // Điền sẵn cho tiện test
  final _passController = TextEditingController(text: "P@ssword123");
  final _formKey = GlobalKey<FormState>();

  @override
  Widget build(BuildContext context) {
    // Lấy trạng thái từ AuthProvider
    final authProvider = Provider.of<AuthProvider>(context);

    return Scaffold(
      body: Center(
        child: SingleChildScrollView(
          padding: const EdgeInsets.all(24),
          child: Form(
            key: _formKey,
            child: Column(
              mainAxisAlignment: MainAxisAlignment.center,
              children: [
                // 1. Logo / Icon
                const Icon(Icons.sports_tennis, size: 80, color: Colors.green),
                const SizedBox(height: 16),
                const Text(
                  "VỢT THỦ PHỐ NÚI",
                  style: TextStyle(fontSize: 24, fontWeight: FontWeight.bold, color: Colors.green),
                ),
                const Text("Mobile Edition", style: TextStyle(color: Colors.grey)),
                
                const SizedBox(height: 40),

                // 2. Input Email
                TextFormField(
                  controller: _emailController,
                  decoration: const InputDecoration(
                    labelText: "Email",
                    prefixIcon: Icon(Icons.email),
                    border: OutlineInputBorder(),
                  ),
                  validator: (val) => val!.isEmpty ? "Vui lòng nhập Email" : null,
                ),
                const SizedBox(height: 16),

                // 3. Input Password
                TextFormField(
                  controller: _passController,
                  obscureText: true,
                  decoration: const InputDecoration(
                    labelText: "Mật khẩu",
                    prefixIcon: Icon(Icons.lock),
                    border: OutlineInputBorder(),
                  ),
                  validator: (val) => val!.isEmpty ? "Vui lòng nhập mật khẩu" : null,
                ),
                const SizedBox(height: 24),

                // 4. Hiển thị lỗi nếu có
                if (authProvider.errorMessage != null)
                  Padding(
                    padding: const EdgeInsets.only(bottom: 16),
                    child: Text(
                      authProvider.errorMessage!,
                      style: const TextStyle(color: Colors.red),
                    ),
                  ),

                // 5. Nút Đăng nhập
                SizedBox(
                  width: double.infinity,
                  height: 50,
                  child: ElevatedButton(
                    style: ElevatedButton.styleFrom(backgroundColor: Colors.green),
                    onPressed: authProvider.isLoading
                        ? null // Vô hiệu hóa nút khi đang load
                        : () async {
                            if (_formKey.currentState!.validate()) {
                              // Gọi hàm login bên Provider
                              bool success = await authProvider.login(
                                _emailController.text,
                                _passController.text,
                              );
                              
                              if (success && mounted) {
                                // Chuyển sang màn hình chính (HomeScreen)
                                Navigator.pushReplacement(
                                  context,
                                  MaterialPageRoute(builder: (_) => const HomeScreen()),
                                );
                              }
                            }
                          },
                    child: authProvider.isLoading
                        ? const CircularProgressIndicator(color: Colors.white)
                        : const Text("ĐĂNG NHẬP", style: TextStyle(fontSize: 18, color: Colors.white)),
                  ),
                ),
              ],
            ),
          ),
        ),
      ),
    );
  }
}