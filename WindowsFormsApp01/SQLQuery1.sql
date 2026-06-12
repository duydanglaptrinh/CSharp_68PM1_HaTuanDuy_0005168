

CREATE DATABASE QLSinhVienCSharp;
GO

-- Sử dụng database
USE QLSinhVienCSharp;
GO

-- Tạo bảng Classes
CREATE TABLE Classes (
    ClassId VARCHAR(50) NOT NULL,
    ClassName NVARCHAR(100) NOT NULL,
    Note NVARCHAR(255) NULL,

    CONSTRAINT PK_Classes PRIMARY KEY (ClassId)
);
GO

-- Tạo bảng Students
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

-- Thêm dữ liệu mẫu cho lớp
INSERT INTO Classes (ClassId, ClassName, Note)
VALUES
('68PM1', N'Lớp 68PM1', 'abc'),
('68PM2', N'Lớp 68PM2', 'xyz');
GO

-- Thêm dữ liệu mẫu cho sinh viên
INSERT INTO Students (MSSV, FullName, DateOfBirth, Gender, ClassId)
VALUES
('SV01', N'Nguyễn Văn A', '2006-05-15', N'Nam', '68PM1'),
('SV02', N'Trần Thị B', '2006-08-20', N'Nữ', '68PM2');
GO

-- Kiểm tra dữ liệu
SELECT * FROM Classes;
SELECT * FROM Students;
GO
DELETE FROM Classes

INSERT INTO Classes(ClassId, ClassName)
VALUES
(1, '68PM1'),
(2, '68PM2')

DELETE FROM Students;
DELETE FROM Classes;

INSERT INTO Classes (ClassId, ClassName)
VALUES
('68PM1', N'Lớp 68PM1'),
('68PM2', N'Lớp 68PM2');

INSERT INTO Students (MSSV, FullName, DateOfBirth, Gender, ClassId)
VALUES
('SV01', N'Nguyễn Văn A', '2006-05-15', N'Nam', '68PM1'),
('SV02', N'Trần Thị B', '2006-08-20', N'Nữ', '68PM2');