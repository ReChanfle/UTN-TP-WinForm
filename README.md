Catalog Management Application
📌 Overview

This project is a desktop application designed to manage items in a commercial catalog. It is built to be generic and adaptable to any type of business. The stored data can later be consumed by external services such as websites, e-commerce platforms, mobile applications, or digital/printed catalogs.

⚠️ External integrations are not part of this project, but they define its usage context.

🚀 Features
List all catalog items
Search items using multiple criteria
Add new items
Edit existing items
Delete items
View detailed item information
📦 Item Structure

Each item includes the following attributes:

Code
Name
Description
Brand (selected from a dropdown list)
Category (selected from a dropdown list)
Images (one or multiple, no limit)
Price
🧩 Additional Management

The application also allows:

Managing Brands
Managing Categories
Associating multiple images to a single item
🗄️ Persistence

All data is stored in an existing database provided with the project.

🏗️ Development Stages
Stage 1
Design domain model classes
Build application windows (UI)
Implement navigation between views
Stage 2
Implement database integration
Add validations and business logic
Enable full CRUD functionality
🛠️ Technologies (example — edit this)
C#
.NET
SQL Server
Windows Forms / WPF
