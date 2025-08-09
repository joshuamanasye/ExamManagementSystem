-- 1. Insert Users (make sure to add Discriminator column)
INSERT INTO [Users] (UserName, Password, Role, Discriminator)
VALUES
('admin1', 'adminpass', 0, 'User'), -- Id 1
('dept1', 'deptpass', 1, 'User'), -- Id 2
('sched1', 'schedpass', 2, 'User'), -- Id 3
('qm1', 'qmpass', 3, 'User'), -- Id 4 (QuestionMaker)
('print1', 'printpass', 4, 'User'), -- Id 5
('invig1', 'invigpass', 5, 'User'), -- Id 6
('lect1', 'lectpass', 6, 'User'), -- Id 7 (Lecturer)
('stud1', 'studpass', 7, 'User'), -- Id 8
('stud2', 'studpass', 7, 'User'), -- Id 9
('stud3', 'studpass', 7, 'User'); -- Id 10

-- 2. Insert Rooms
INSERT INTO Rooms (Number, IsAvailable, Capacity)
VALUES
('A101', 1, 30),
('B202', 1, 50),
('C303', 0, 40);

-- 3. Insert Exam Files (link QuestionMakerId = 4)
INSERT INTO ExamFiles (FilePath, Status, QuestionMakerId)
VALUES
('files/exam_math.pdf', 1, 4),  
('files/exam_history.pdf', 0, 4),
('files/exam_science.pdf', 2, 4);

-- 4. Insert Courses (LecturerId = 7)
INSERT INTO Courses (Name, LecturerId)
VALUES
('Mathematics 101', 7), -- Id 1
('History 201', 7),     -- Id 2
('Science 301', 7);     -- Id 3

-- 5. Insert StudentCourse mapping
INSERT INTO StudentCourses (CourseId, StudentId)
VALUES
(1, 8),
(1, 9),
(2, 9),
(2, 10),
(3, 8),
(3, 10);

-- 6. Insert Exams (Duration as TIME format, link valid CourseId/RoomId/ExamFileId)
INSERT INTO Exams (CourseId, DurationMinutes, Date, RoomId, ExamFileId)
VALUES
(1, '01:30:00', '2025-08-15 09:00:00', 1, 1),
(2, '01:00:00', '2025-08-16 13:00:00', 2, 2),
(3, '02:00:00', '2025-08-17 10:00:00', 3, 3);

-- 7. Insert Grades (link valid ExamId/StudentId)
INSERT INTO Grades (ExamId, StudentId, Score)
VALUES
(1, 8, 85),
(1, 9, 90),
(2, 9, 88),
(2, 10, 75),
(3, 8, 92),
(3, 10, 80);

-- 8. Insert Attendance
INSERT INTO Attendances (ExamId, StudentId, IsPresent)
VALUES
(1, 8, 1),
(1, 9, 1),
(2, 9, 1),
(2, 10, 0),
(3, 8, 1),
(3, 10, 1);
