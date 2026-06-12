-- ========================================================
-- FILE TRUY VẤN TOÀN DIỆN CƠ SỞ DỮ LIỆU QLSinhVienCSharp
-- Hướng dẫn: Nhấn Ctrl + A để chọn tất cả, sau đó nhấn Execute (hoặc Ctrl + Shift + E)
-- ========================================================

-- Lưu ý: Nếu máy bạn chưa tạo Database lần nào, hãy bỏ comment 2 dòng dưới đây để chạy:
-- CREATE DATABASE QLSinhVienCSharp;
-- GO

USE QLSinhVienCSharp;
GO




CREATE TABLE Classes (
    ClassId VARCHAR(50) NOT NULL,
    ClassName NVARCHAR(100) NOT NULL,
    Note NVARCHAR(255) NULL,

    CONSTRAINT PK_Classes PRIMARY KEY (ClassId)
);
GO


CREATE TABLE Students (
    MSSV VARCHAR(50) NOT NULL,
    FullName NVARCHAR(100) NOT NULL,
    DateOfBirth DATE NULL,
    Gender NVARCHAR(10) NULL,
    ClassId VARCHAR(50) NOT NULL,

    CONSTRAINT PK_Students PRIMARY KEY (MSSV),

    CONSTRAINT FK_Students_Classes
        FOREIGN KEY (ClassId)
        REFERENCES Classes(ClassId)
        ON DELETE CASCADE
);
GO

INSERT INTO Classes (ClassId, ClassName, Note)
VALUES
('68PM1', N'Phần mềm 1 - K68', N'Lớp chuyên ngành C#'),
('68PM2', N'Phần mềm 2 - K68', N'Lớp chuyên ngành Java'),
('68PM3', N'Phần mềm 3 - K68', N'Lớp chuyên ngành Web'),
('68PM4', N'Phần mềm 4 - K68', N'Lớp chuyên ngành Mobile'),
('68PM5', N'Phần mềm 5 - K68', N'Lớp Trí tuệ nhân tạo (AI)'),
('67PM1', N'Phần mềm 1 - K67', N'Sinh viên khóa trước'),
('67PM2', N'Phần mềm 2 - K67', N'Sinh viên khóa trước');
GO


INSERT INTO Students (MSSV, FullName, DateOfBirth, Gender, ClassId)
VALUES
('SV001', N'Nguyễn Văn An', '2004-01-15', N'Nam', '68PM1'),
('SV002', N'Trần Thị Bình', '2004-02-20', N'Nữ', '68PM1'),
('SV003', N'Lê Hoàng Cường', '2004-03-10', N'Nam', '68PM1'),
('SV004', N'Phạm Thị Dung', '2004-04-05', N'Nữ', '68PM1'),
('SV005', N'Hoàng Văn Em', '2004-05-12', N'Nam', '68PM2'),
('SV006', N'Ngô Thị Phương', '2004-06-18', N'Nữ', '68PM2'),
('SV007', N'Vũ Ngọc Giáp', '2004-07-22', N'Nam', '68PM2'),
('SV008', N'Đặng Thu Hà', '2004-08-30', N'Nữ', '68PM3'),
('SV009', N'Bùi Xuân Ý', '2004-09-14', N'Nam', '68PM3'),
('SV010', N'Đỗ Thị Kim', '2004-10-08', N'Nữ', '68PM3'),
('SV011', N'Hồ Quang Linh', '2004-11-25', N'Nam', '68PM4'),
('SV012', N'Dương Thị Mai', '2004-12-02', N'Nữ', '68PM4'),
('SV013', N'Lý Hải Nam', '2003-01-19', N'Nam', '67PM1'),
('SV014', N'Đào Thị Oanh', '2003-02-14', N'Nữ', '67PM1'),
('SV015', N'Đoàn Văn Phong', '2003-03-27', N'Nam', '67PM2');
GO


SELECT * FROM Classes;
SELECT * FROM Students;
GO