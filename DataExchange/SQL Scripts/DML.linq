<Query Kind="SQL">
  <Connection>
    <ID>d30dde50-1172-4d49-aeb0-33a70c36f40f</ID>
    <Persist>true</Persist>
    <Server>AS-KNATZ-US</Server>
    <Database>SSDMalta</Database>
    <DisplayName>SSDDatabase</DisplayName>
  </Connection>
</Query>

print '*********----START!----*********'

print 'INSERTING VALUES'

--GENDER
insert Genders values ('Female')
insert Genders values ('Male')
insert Genders values ('Other')

--TOWN
insert Towns values ('Paola')
insert Towns values ('Fgura')
insert Towns values ('Mosta')
insert Towns values ('Valletta')
insert Towns values ('Zejtun')
insert Towns values ('Birgu')
insert Towns values ('Qormi')
insert Towns values ('Gudja')
insert Towns values ('Ħad-Dingli')

--ROLE
insert AspNetRoles values ('894EDD4C-6E08-4502-BF29-E45E272A6242' , 'Administrator', 'AspNetRole')
insert AspNetRoles values ('99C7E43E-DB45-4986-9D29-82C7E27F6C31' , 'Customer', 'AspNetRole')
insert AspNetRoles values ('A3495224-155D-4EA0-A3A4-1D218BB9F98F' , 'Guest', 'AspNetRole')

--MENU
insert Menus values ('Home', 1000, null,'The home page','Index','Home', null)
insert Menus values ('Gallery', 2000, null,'The gallery viewing page','Index','Gallery', null)
insert Menus values ('Dashboard', 3000, null,'The control page','Index','Home', null)
insert Menus values ('API', 4000, null,'The API services link', null, null, 'http://localhost:60934/')
insert Menus values ('About', 5000, null,'The about page','About','Home', null)
insert Menus values ('Contact', 6000, null,'The contact page','Contact','Home', null)

--Sub Menu Items -> Dashboard
insert Menus values ('User Management', 3001, 3,'The user management page','Index','Account', null)
insert Menus values ('Gallery Management', 3002, 3,'The gallery management page','Index','Gallery', null)
insert Menus values ('Profile', 3003, 3,'The account management page','Index','Manage', null)
	
--Admin Menu
insert MenuRoles values (1, '894EDD4C-6E08-4502-BF29-E45E272A6242') --Home
insert MenuRoles values (2, '894EDD4C-6E08-4502-BF29-E45E272A6242') --Gallery View
insert MenuRoles values (3, '894EDD4C-6E08-4502-BF29-E45E272A6242') --Dashboard
insert MenuRoles values (4, '894EDD4C-6E08-4502-BF29-E45E272A6242') --API
insert MenuRoles values (5, '894EDD4C-6E08-4502-BF29-E45E272A6242') --About
insert MenuRoles values (6, '894EDD4C-6E08-4502-BF29-E45E272A6242') --Contact

--Sub Elements
insert MenuRoles values (7, '894EDD4C-6E08-4502-BF29-E45E272A6242') --Dashboard = Users Manager
insert MenuRoles values (8, '894EDD4C-6E08-4502-BF29-E45E272A6242') --Dashboard = Gallery Manager
insert MenuRoles values (9, '894EDD4C-6E08-4502-BF29-E45E272A6242') --Dashboard = Users Profile

--Customer Menu
insert MenuRoles values (1, '99C7E43E-DB45-4986-9D29-82C7E27F6C31') --Home
insert MenuRoles values (2, '99C7E43E-DB45-4986-9D29-82C7E27F6C31') --Gallery View
insert MenuRoles values (3, '99C7E43E-DB45-4986-9D29-82C7E27F6C31') --Dashboard
insert MenuRoles values (4, '99C7E43E-DB45-4986-9D29-82C7E27F6C31') --API
insert MenuRoles values (5, '99C7E43E-DB45-4986-9D29-82C7E27F6C31') --About
insert MenuRoles values (6, '99C7E43E-DB45-4986-9D29-82C7E27F6C31') --Contact
--Sub Elements
insert MenuRoles values (9, '99C7E43E-DB45-4986-9D29-82C7E27F6C31') --Dashboard = Gallery Manager
insert MenuRoles values (8, '99C7E43E-DB45-4986-9D29-82C7E27F6C31') --Dashboard = Users Profile

--Guest Menu
insert MenuRoles values (1, 'A3495224-155D-4EA0-A3A4-1D218BB9F98F') --Home
insert MenuRoles values (4, 'A3495224-155D-4EA0-A3A4-1D218BB9F98F') --API
insert MenuRoles values (5, 'A3495224-155D-4EA0-A3A4-1D218BB9F98F') --About
insert MenuRoles values (6, 'A3495224-155D-4EA0-A3A4-1D218BB9F98F') --Contact


print '*********----DONE!-----*********'