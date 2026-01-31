import 'package:flutter/foundation.dart';

class ApiConfig {
  // IP bạn vừa tìm được: 172.30.112.1
  // Cổng đã cấu hình trong Docker Compose: 8080
  
  static String get baseUrl {
    if (kIsWeb) {
      // Nếu chạy Web trên cùng máy tính host Docker
      return "http://localhost:8080/api"; 
    } else {
      // Dùng IP LAN để cả máy ảo và máy thật đều truy cập được
      return "http://172.30.112.1:8080/api";
    }
  }
}