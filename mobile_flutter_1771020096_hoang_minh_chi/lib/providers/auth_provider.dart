import 'package:flutter/material.dart';
import '../models/user_model.dart';
import '../services/auth_service.dart';
import 'package:flutter_secure_storage/flutter_secure_storage.dart';

class AuthProvider extends ChangeNotifier {
  final AuthService _authService = AuthService();
  final _storage = const FlutterSecureStorage();

  UserModel? _user;
  bool _isLoading = false;
  String? _errorMessage;

  UserModel? get user => _user;
  bool get isLoading => _isLoading;
  String? get errorMessage => _errorMessage;

  // Hàm đăng nhập
  Future<bool> login(String email, String password) async {
    _isLoading = true;
    _errorMessage = null;
    notifyListeners(); // Báo cho UI hiện loading

    try {
      final user = await _authService.login(email, password);
      if (user != null) {
        _user = user;
        // Lưu token vào bộ nhớ máy để lần sau tự đăng nhập
        await _storage.write(key: 'jwt_token', value: user.token);
        _isLoading = false;
        notifyListeners();
        return true; // Đăng nhập thành công
      }
    } catch (e) {
      _errorMessage = e.toString().replaceAll("Exception:", "").trim();
    }

    _isLoading = false;
    notifyListeners(); // Báo cho UI tắt loading và hiện lỗi (nếu có)
    return false; // Đăng nhập thất bại
  }

  // Hàm đăng xuất
  Future<void> logout() async {
    await _storage.delete(key: 'jwt_token');
    _user = null;
    notifyListeners();
  }
}