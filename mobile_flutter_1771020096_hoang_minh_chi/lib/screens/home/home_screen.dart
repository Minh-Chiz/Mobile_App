import 'package:flutter/material.dart';
import 'package:provider/provider.dart';
import '../../providers/auth_provider.dart';
import '../auth/login_screen.dart';
import '../booking/booking_screen.dart';

class HomeScreen extends StatelessWidget {
  const HomeScreen({super.key});

  @override
  Widget build(BuildContext context) {
    // Lấy thông tin user từ AuthProvider
    final user = Provider.of<AuthProvider>(context).user;

    // Danh sách các chức năng trên Dashboard
    final List<Map<String, dynamic>> menuItems = [
      {'icon': Icons.sports_tennis, 'title': 'Đặt sân', 'color': Colors.green},
      {'icon': Icons.history, 'title': 'Lịch sử', 'color': Colors.blue},
      {'icon': Icons.account_balance_wallet, 'title': 'Nạp tiền', 'color': Colors.orange},
      {'icon': Icons.person, 'title': 'Hồ sơ', 'color': Colors.purple},
      {'icon': Icons.notifications, 'title': 'Thông báo', 'color': Colors.red},
      {'icon': Icons.settings, 'title': 'Cài đặt', 'color': Colors.grey},
    ];

    return Scaffold(
      backgroundColor: Colors.grey[100], // Màu nền nhẹ nhàng
      appBar: AppBar(
        title: const Text("PCM Pickleball"),
        centerTitle: true,
        actions: [
          IconButton(
            icon: const Icon(Icons.logout),
            onPressed: () {
              // Xử lý đăng xuất
              Provider.of<AuthProvider>(context, listen: false).logout();
              Navigator.pushReplacement(
                context, 
                MaterialPageRoute(builder: (_) => const LoginScreen())
              );
            },
          )
        ],
      ),
      body: Column(
        children: [
          // Phần 1: Thẻ thông tin người dùng
          Container(
            width: double.infinity,
            padding: const EdgeInsets.all(20),
            decoration: BoxDecoration(
              color: Colors.green.shade600,
              borderRadius: const BorderRadius.only(
                bottomLeft: Radius.circular(30),
                bottomRight: Radius.circular(30),
              ),
            ),
            child: Column(
              crossAxisAlignment: CrossAxisAlignment.start,
              children: [
                Text(
                  "Xin chào, ${user?.fullName ?? 'Bạn'}!",
                  style: const TextStyle(color: Colors.white, fontSize: 18),
                ),
                const SizedBox(height: 10),
                Text(
                  "${user?.walletBalance ?? 0} đ",
                  style: const TextStyle(
                    color: Colors.white, 
                    fontSize: 28, 
                    fontWeight: FontWeight.bold
                  ),
                ),
                const SizedBox(height: 5),
                Container(
                  padding: const EdgeInsets.symmetric(horizontal: 10, vertical: 5),
                  decoration: BoxDecoration(
                    color: Colors.white.withOpacity(0.2),
                    borderRadius: BorderRadius.circular(10),
                  ),
                  child: Text(
                    "Hạng: ${user?.tier ?? 'Thành viên'}",
                    style: const TextStyle(color: Colors.white, fontWeight: FontWeight.bold),
                  ),
                ),
              ],
            ),
          ),
          
          const SizedBox(height: 20),

          // Phần 2: Lưới các chức năng (Dashboard Grid)
          Expanded(
            child: Padding(
              padding: const EdgeInsets.symmetric(horizontal: 15),
              child: GridView.builder(
                itemCount: menuItems.length,
                gridDelegate: const SliverGridDelegateWithFixedCrossAxisCount(
                  crossAxisCount: 2, // Số cột
                  crossAxisSpacing: 15,
                  mainAxisSpacing: 15,
                  childAspectRatio: 1.5, // Tỷ lệ chiều rộng/cao của ô
                ),
                itemBuilder: (context, index) {
                  return _buildDashboardItem(
                    icon: menuItems[index]['icon'],
                    title: menuItems[index]['title'],
                    color: menuItems[index]['color'],
                    onTap: () {
                      if (index == 0) { // Nếu bấm vào "Đặt sân" (Item đầu tiên)
                        Navigator.push(
                          context,
                          MaterialPageRoute(builder: (context) => const BookingScreen()),
                        );
                      } else {
                        ScaffoldMessenger.of(context).showSnackBar(
                          SnackBar(content: Text("Tính năng ${menuItems[index]['title']} đang phát triển")),
                        );
                      }
                    },
                  ); // Đã thêm dấu chấm phẩy ở đây để sửa lỗi
                },
              ),
            ),
          ),
        ],
      ),
    );
  }

  // Widget con để hiển thị từng ô chức năng
  Widget _buildDashboardItem({
    required IconData icon, 
    required String title, 
    required Color color,
    required VoidCallback onTap,
  }) {
    return GestureDetector(
      onTap: onTap,
      child: Container(
        decoration: BoxDecoration(
          color: Colors.white,
          borderRadius: BorderRadius.circular(15),
          boxShadow: [
            BoxShadow(
              color: Colors.grey.withOpacity(0.2),
              spreadRadius: 2,
              blurRadius: 5,
              offset: const Offset(0, 3),
            ),
          ],
        ),
        child: Column(
          mainAxisAlignment: MainAxisAlignment.center,
          children: [
            CircleAvatar(
              radius: 25,
              backgroundColor: color.withOpacity(0.1),
              child: Icon(icon, size: 30, color: color),
            ),
            const SizedBox(height: 10),
            Text(
              title,
              style: const TextStyle(
                fontSize: 16, 
                fontWeight: FontWeight.bold,
                color: Colors.black87
              ),
            ),
          ],
        ),
      ),
    );
  }
}