import 'package:flutter/material.dart';

class BookingScreen extends StatefulWidget {
  const BookingScreen({super.key});

  @override
  State<BookingScreen> createState() => _BookingScreenState();
}

class _BookingScreenState extends State<BookingScreen> {
  // Dữ liệu giả lập (Sau này sẽ lấy từ API)
  DateTime _selectedDate = DateTime.now();
  int _selectedCourtIndex = 0;
  int _selectedTimeSlotIndex = -1;

  final List<String> _courts = ["Sân A (Tiêu chuẩn)", "Sân B (Tiêu chuẩn)", "Sân C (VIP)", "Sân D (Tập luyện)"];
  
  final List<String> _timeSlots = [
    "06:00 - 07:00", "07:00 - 08:00", "08:00 - 09:00",
    "16:00 - 17:00", "17:00 - 18:00", "18:00 - 19:00",
    "19:00 - 20:00", "20:00 - 21:00"
  ];

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(
        title: const Text("Đặt Sân Pickleball"),
        backgroundColor: Colors.green,
        foregroundColor: Colors.white,
      ),
      body: SingleChildScrollView(
        padding: const EdgeInsets.all(16.0),
        child: Column(
          crossAxisAlignment: CrossAxisAlignment.start,
          children: [
            // 1. CHỌN NGÀY
            _buildSectionTitle("1. Chọn ngày chơi"),
            const SizedBox(height: 10),
            Container(
              padding: const EdgeInsets.all(12),
              decoration: BoxDecoration(
                border: Border.all(color: Colors.grey.shade300),
                borderRadius: BorderRadius.circular(10),
              ),
              child: Row(
                mainAxisAlignment: MainAxisAlignment.spaceBetween,
                children: [
                  Text(
                    "Ngày: ${_selectedDate.day}/${_selectedDate.month}/${_selectedDate.year}",
                    style: const TextStyle(fontSize: 16),
                  ),
                  ElevatedButton.icon(
                    onPressed: _pickDate,
                    icon: const Icon(Icons.calendar_today, size: 18),
                    label: const Text("Đổi ngày"),
                    style: ElevatedButton.styleFrom(
                      backgroundColor: Colors.green.shade50,
                      foregroundColor: Colors.green,
                    ),
                  )
                ],
              ),
            ),
            
            const SizedBox(height: 25),

            // 2. CHỌN SÂN
            _buildSectionTitle("2. Chọn sân"),
            const SizedBox(height: 10),
            SizedBox(
              height: 120, // Chiều cao khu vực chọn sân
              child: ListView.builder(
                scrollDirection: Axis.horizontal,
                itemCount: _courts.length,
                itemBuilder: (context, index) {
                  final isSelected = _selectedCourtIndex == index;
                  return GestureDetector(
                    onTap: () => setState(() => _selectedCourtIndex = index),
                    child: Container(
                      width: 140,
                      margin: const EdgeInsets.only(right: 12),
                      decoration: BoxDecoration(
                        color: isSelected ? Colors.green : Colors.white,
                        borderRadius: BorderRadius.circular(12),
                        border: Border.all(
                          color: isSelected ? Colors.green : Colors.grey.shade300,
                          width: 2,
                        ),
                        boxShadow: [
                          if(!isSelected)
                            BoxShadow(
                              color: Colors.grey.shade200,
                              blurRadius: 4,
                              offset: const Offset(0, 2),
                            )
                        ]
                      ),
                      child: Column(
                        mainAxisAlignment: MainAxisAlignment.center,
                        children: [
                          Icon(
                            Icons.sports_tennis, 
                            size: 40, 
                            color: isSelected ? Colors.white : Colors.green
                          ),
                          const SizedBox(height: 8),
                          Text(
                            _courts[index],
                            textAlign: TextAlign.center,
                            style: TextStyle(
                              color: isSelected ? Colors.white : Colors.black87,
                              fontWeight: FontWeight.bold,
                            ),
                          ),
                        ],
                      ),
                    ),
                  );
                },
              ),
            ),

            const SizedBox(height: 25),

            // 3. CHỌN GIỜ
            _buildSectionTitle("3. Chọn khung giờ"),
            const SizedBox(height: 10),
            GridView.builder(
              shrinkWrap: true, // Để GridView nằm gọn trong Column
              physics: const NeverScrollableScrollPhysics(),
              gridDelegate: const SliverGridDelegateWithFixedCrossAxisCount(
                crossAxisCount: 3,
                childAspectRatio: 2.5,
                crossAxisSpacing: 10,
                mainAxisSpacing: 10,
              ),
              itemCount: _timeSlots.length,
              itemBuilder: (context, index) {
                final isSelected = _selectedTimeSlotIndex == index;
                return ChoiceChip(
                  label: Text(_timeSlots[index]),
                  selected: isSelected,
                  selectedColor: Colors.green.shade100,
                  labelStyle: TextStyle(
                    color: isSelected ? Colors.green.shade900 : Colors.black,
                    fontSize: 12,
                    fontWeight: isSelected ? FontWeight.bold : FontWeight.normal
                  ),
                  onSelected: (selected) {
                    setState(() => _selectedTimeSlotIndex = selected ? index : -1);
                  },
                );
              },
            ),
          ],
        ),
      ),
      bottomNavigationBar: Container(
        padding: const EdgeInsets.all(16),
        decoration: BoxDecoration(
          color: Colors.white,
          boxShadow: [BoxShadow(color: Colors.black12, blurRadius: 10, offset: const Offset(0, -2))],
        ),
        child: SafeArea(
          child: ElevatedButton(
            onPressed: _selectedTimeSlotIndex != -1 
              ? () {
                  // Xử lý logic đặt sân tại đây
                  ScaffoldMessenger.of(context).showSnackBar(
                    const SnackBar(content: Text("Đặt sân thành công!")),
                  );
                  Navigator.pop(context); // Quay về trang chủ
                } 
              : null, // Disable nút nếu chưa chọn giờ
            style: ElevatedButton.styleFrom(
              backgroundColor: Colors.green,
              foregroundColor: Colors.white,
              padding: const EdgeInsets.symmetric(vertical: 15),
              shape: RoundedRectangleBorder(borderRadius: BorderRadius.circular(10)),
            ),
            child: const Text("XÁC NHẬN ĐẶT SÂN", style: TextStyle(fontSize: 16, fontWeight: FontWeight.bold)),
          ),
        ),
      ),
    );
  }

  // Widget tiêu đề nhỏ
  Widget _buildSectionTitle(String title) {
    return Text(
      title,
      style: const TextStyle(fontSize: 18, fontWeight: FontWeight.bold, color: Colors.black87),
    );
  }

  // Hàm hiển thị lịch chọn ngày
  Future<void> _pickDate() async {
    final DateTime? picked = await showDatePicker(
      context: context,
      initialDate: _selectedDate,
      firstDate: DateTime.now(),
      lastDate: DateTime.now().add(const Duration(days: 30)),
    );
    if (picked != null && picked != _selectedDate) {
      setState(() {
        _selectedDate = picked;
      });
    }
  }
}