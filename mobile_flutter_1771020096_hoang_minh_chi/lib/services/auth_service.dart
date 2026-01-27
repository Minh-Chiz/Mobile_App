import 'package:dio/dio.dart';
import '../configs/api_config.dart';
import '../models/user_model.dart';

class AuthService {
  final Dio _dio = Dio();

  Future<UserModel?> login(String email, String password) async {
    try {
      final response = await _dio.post(
        '${ApiConfig.baseUrl}/Auth/login',
        data: {
          "email": email,
          "password": password,
        },
      );

      if (response.statusCode == 200) {
        return UserModel.fromJson(response.data);
      }
      return null;
    } catch (e) {
      print("Lỗi đăng nhập: $e");
      throw Exception("Sai email hoặc mật khẩu");
    }
  }
}