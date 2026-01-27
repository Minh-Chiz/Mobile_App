import 'package:flutter/foundation.dart'; // Cần import thư viện này để dùng kIsWeb

class ApiConfig {
  // Máy ảo Android dùng 10.0.2.2
  // Web hoặc iOS Simulator dùng localhost
  // Máy thật dùng IP LAN
  
  static String get baseUrl {
    if (kIsWeb) {
      // Nếu đang chạy trên Web -> Dùng localhost
      return "http://localhost:5186/api"; 
    } else {
      // Nếu chạy trên Android Emulator -> Dùng 10.0.2.2
      // (Hoặc đổi thành IP LAN nếu bạn test máy thật)
      return "http://10.0.2.2:5186/api";
    }
  }
}