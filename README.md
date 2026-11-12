# Student Management System

A console-based student management application developed in C# using .NET Framework 4.7.2.

The application allows users to manage student records, track grades, calculate averages, and store data between sessions. It was developed as a personal project to practice object-oriented programming, collections, file handling, and input validation.

## Features

- View all students and their grades.
- Search for students by first and last name.
- Add new students.
- Add grades between 1 and 10.
- Remove existing students.
- Calculate student grade averages.
- Display students sorted by average grade in descending order.
- Prevent duplicate student records based on first and last name.
- Validate user input, including student names and grades.
- Automatically load student data when the application starts.
- Save student data when exiting through the console menu.
- Detect invalid data in the storage file and handle file-related errors.
- Create a backup of the previous data file when saving changes.

## Technologies

- C#
- .NET Framework 4.7.2
- Object-oriented programming (OOP)
- Generic collections (`List<T>`)
- LINQ
- File I/O
- Exception handling and input validation

## Data Storage

Student records are stored in a text file named `Studentai.txt`, located in the application's executable directory.

Each record follows this format:

`FirstName;LastName;Grade1,Grade2,Grade3`

Example:

`Jonas;Jonaitis;8,9,10`

Students without grades are also supported.

To reduce the risk of data loss, the application:

1. Writes updated student records to a temporary `.tmp` file.
2. Replaces the existing data file after the temporary file has been written.
3. Creates a `.bak` backup containing the previous version of the data file.
4. Creates a new data file if one does not already exist.

If invalid student data is detected during loading, the application reports an error instead of overwriting the original file.

## How to Run

**Requirements:**

- Windows
- Visual Studio with .NET desktop development support
- .NET Framework 4.7.2 targeting pack

**Instructions:**

1. Clone or download this repository.
2. Open `StudentManagementSystem.slnx` in a compatible version of Visual Studio.
3. Build the solution.
4. Run the application.
5. Use the console menu to manage student records.
6. Select `0` to exit and save changes.

The application creates `Studentai.txt` in its executable directory when data is saved for the first time. During development, this directory is typically `bin/Debug/`.

## What I Learned

Through this project, I gained practical experience in:

- Designing and using classes and objects.
- Applying basic OOP principles and encapsulation.
- Managing data with lists and collections.
- Separating functionality into reusable methods.
- Reading, validating, and writing structured text files.
- Handling exceptions and invalid user input.
- Using LINQ to sort collections.
- Improving code readability and maintainability.

## Future Improvements

- Integrate a SQL database for data storage.
- Add automated unit tests.
- Allow editing existing student information.
- Implement additional search and filtering options.

---

**Status:** Core functionality implemented and manually tested.