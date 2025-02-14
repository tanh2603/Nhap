Các bước setup trước khi sử dụng
1. Tải xuống tất cả các file về
2. Mở SQL Server ManagementStudio
  2.1. Đăng nhập vào máy chủ của máy (Copy tên của máy chủ)
  2.2. Chuột phải vào folder "Databases" trong máy chủ vừa đăng nhập
  2.3 Chọn "Restore Database..."
  2.3.1 Phần Source chọn "Device" > chọn file BANDOAN vừa tải về và nhấn OK
  2.4 **Nếu Restore không được thì nhấn vào New Query (chạy chế độ master) chạy lệnh sau:

	RESTORE DATABASE BANDOAN  
	FROM DISK = 'Chi tiết nơi chứa file Backup\BANDOAN.bak'  
	WITH RECOVERY, MOVE 'LogicalDataFileName' TO 'Nơi lưu file\BANDOAN.mdf',
	MOVE 'LogicalLogFileName' TO 'Nơi lưu file\BANDOAN_log.ldf';**(Khuyết khích nên luu chung với file chứa file backup)

	LogicalDataFileName và LogicalLogFileName là tên logic của file dữ liệu và log trong backup. Bạn cần kiểm tra bằng lệnh 	sau trước khi restore:

	RESTORE FILELISTONLY FROM DISK = 'Chi tiết nơi chứa file Backup\BANDOAN.bak'
	
	**Chi tiết nơi chứ file backup vd: D:\Documents\BANDOAN.bak

3. Mở Visual Studio 2022 (Tuỳ thuộc vào phiên bản máy của bạn, khuyến khích nên bản mới nhất để không xãy ra lỗi không mong muốn)
  3.1 Tại phần Menu phía trên chọn "View" > "Orther Windows" > "Data Sources"
  3.2 Tại tab Data Sources chọn "biểu tượng dấu cộng" để thêm CSDL
  3.3 Cửa sổ mới hiện ra Chọn "Database" next "Dataset" next "New Connection"
  3.4 Cửa sổ mới hiện ra ở Dán đường dẫn mà đã copy (ở bước 2.1) "Server Name"
  3.4 Phần Authentication chọn "Windows Authentication" Hoặc chọn "SQL Server Authentication"
  3.5 Bạn nhập thông tin tìa khoản của máy chủ của bạn (mà bạn đã đăng nhập trước). Chọn vào checkbox "Trust Server Certificate" (bắt buộc) và "Save my password" (nếu muốn)
  3.6 Tại phần "Select or enter a database name" chọn đúng tên CSDL là BANDOAN
  3.7 Có thể "Test Connection" để kiểm tra kết nối và OK
  3.8 Mở cửa sổ "Server Explorer" chuột phải vào tên Data Connections vừa kết nối chọn Properties
  3.9 Chỗ mục Connection String copy đường dẫn Data Source 
4. Mở cửa sổ "Solution Explorer" (khi này bạn sẽ thấy một File mới vừa được tạo "tên database".xsd là thành công kết nối)
  4.1. Vào Solution Explorer chọn mục App.config sửa đoạn code sau:
	<connectionStrings>
		<add name="Quanlybandoan.Properties.Settings.BANDOANConnectionString"
	 	connectionString="Đường dẫn Data Source vừa copy trên 3.9"
	 	providerName="System.Data.SqlClient" />
	</connectionStrings>
  4.2. Vào Solution Explorer chọn thư mục DAO chọn mục DataProvider.cs tại dòng 27 thay đổi đường dân Data Source:
	private string connectionSTR = "Đường dẫn Data Source vừa copy trên 3.9"
5. Nhấn vào Start hoặc F5 để chạy chương trình

***Lưu ý kiểm tra trạng thái của kết nối máy chủ CSDL tại cửa sổ "Server Explorer" nếu thấy "dấu x" thì Refresh lại trên góc.

Để đăng nhập vào hệ thống:
Tài khoản Quản trị viên là: Quanly1
Password: 123
* Trong giao diện chính, chọn vào tên hiện thị để đăng xuất
Em cảm ơn !
