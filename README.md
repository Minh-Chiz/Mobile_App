# MOBILE_FLUTTER_1771020096_HOANG_MINH_CHI
## HỆ THỐNG QUẢN LÝ CLB PICKLEBALL "VỢT THỦ PHỐ NÚI" (PCM)

Đây là đồ án kết thúc môn Lập trình Mobile với Flutter. Hệ thống bao gồm Backend API (ASP.NET Core) và Mobile App (Flutter).

---

### 👨‍💻 Thông tin sinh viên
* **Họ và tên:** Hoàng Minh Chí
* **MSSV:** 1771020096
* **Lớp:** CNTT 17-08
* **Đề tài:** Hệ thống quản lý CLB Pickleball (PCM)

---

### 🛠 Công nghệ sử dụng
#### 1. Backend (Server)
* **Framework:** ASP.NET Core Web API (.NET 8)
* **Database:** SQL Server (sử dụng Entity Framework Core Code-First)
* **Authentication:** JWT (JSON Web Token) & ASP.NET Core Identity
* **Real-time:** SignalR (Cập nhật trạng thái đặt sân, ví tiền)
* **Background Services:** Tự động hủy đơn đặt sân nếu không thanh toán sau 5 phút.

#### 2. Mobile (Client)
* **Framework:** Flutter (Dart)
* **State Management:** Provider
* **Networking:** Dio (có Interceptor xử lý Token)
* **UI Components:** Syncfusion Calendar (Lịch đặt sân), FL Chart.

---

### 🚀 Hướng dẫn cài đặt & Chạy dự án

#### PHẦN 1: BACKEND (ASP.NET CORE)

**Bước 1: Cấu hình Database**
1.  Mở thư mục `BackendAPI_1771020096_HoangMinhChi`.
2.  Mở file `appsettings.json`, kiểm tra chuỗi kết nối (Mặc định dùng LocalDB của Visual Studio, không cần cài SQL Server riêng):
    ```json
    "ConnectionStrings": {
      "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=PcmDb_1771020096;Trusted_Connection=True;MultipleActiveResultSets=true"
    }
    ```

**Bước 2: Khởi tạo Database & Dữ liệu mẫu**
1.  Mở dự án bằng **Visual Studio 2022**.
2.  Vào **Tools** > **NuGet Package Manager** > **Package Manager Console**.
3.  Chạy lệnh sau để cập nhật Database:
    ```powershell
    Update-Database
    ```
4.  Bấm nút **Run (Play)** hoặc nhấn `F5` để chạy Server.
    * Hệ thống sẽ tự động tạo dữ liệu mẫu (Seeding) gồm: 1 Admin, 20 Member, Sân bãi và Giải đấu.
    * **Lưu ý Port:** Kiểm tra xem Swagger chạy ở port nào (Ví dụ: `http://localhost:5186`). Đây là port để config bên Flutter.

---

#### PHẦN 2: MOBILE APP (FLUTTER)

**Bước 1: Cấu hình kết nối API**
1.  Mở thư mục `mobile_flutter_1771020096_hoang_minh_chi` bằng VS Code.
2.  Mở file `lib/configs/api_config.dart`.
3.  Cập nhật `baseUrl` tùy theo môi trường chạy:

    * **Nếu chạy trên Máy ảo Android (Emulator):**
        ```dart
        static const String baseUrl = "[http://10.0.2.2:5186/api](http://10.0.2.2:5186/api)"; // 5186 là port Backend
        ```
    * **Nếu chạy trên Điện thoại thật (Cắm cáp USB):**
        * Bước A: Mở Terminal máy tính chạy lệnh: `adb reverse tcp:5186 tcp:5186`
        * Bước B: Config code:
            ```dart
            static const String baseUrl = "[http://127.0.0.1:5186/api](http://127.0.0.1:5186/api)";
            ```

**Bước 2: Cài đặt thư viện & Chạy App**
1.  Mở Terminal tại thư mục Flutter, chạy lệnh:
    ```bash
    flutter pub get
    ```
2.  Chạy ứng dụng:
    ```bash
    flutter run
    ```

---

### 🔑 Tài khoản Test (Dữ liệu mẫu)

Hệ thống đã tạo sẵn các tài khoản sau để Giảng viên kiểm tra:

| Vai trò | Email | Mật khẩu | Ghi chú |
| :--- | :--- | :--- | :--- |
| **Hội viên (Member)** | `member1@pcm.com` | `P@ssword123` | Có sẵn tiền trong ví, hạng Gold |
| **Hội viên (Member)** | `member2@pcm.com` | `P@ssword123` | Để test đặt trùng lịch |
| **Quản trị (Admin)** | `admin@pcm.com` | `P@ssword123` | Quản lý sân, duyệt nạp tiền |

---

### 📸 Các chức năng chính (Demo)
1.  **Đăng nhập/Đăng ký:** Có lưu Token, tự động đăng nhập lần sau.
2.  **Ví điện tử:** Xem số dư, lịch sử giao dịch.
3.  **Đặt sân (Booking):**
    * Xem lịch trống/bận trực quan.
    * Chọn giờ -> Trừ tiền ví ngay lập tức.
    * Chặn đặt trùng giờ (Server validation).
4.  **Giải đấu:** Xem danh sách giải đấu, bấm tham gia (trừ phí tham dự).

---
*Cảm ơn Thầy/Cô đã xem bài!*
