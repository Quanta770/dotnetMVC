## Set-up
- Download SQL Server Management Studio
- Download SQL server from : https://www.microsoft.com/en-my/sql-server/sql-server-downloads
- Set connection string in appsettings.json
- Check packages are installed via NuGet package manager:
	- EntityFrameworkCore
	- EntityFrameworkCore.SqlServer
	- EntityFrameworkCore.Tools
- Perform *add-migration* and *update-database* to create and seed table.
---
## 1. Introduction to dotnet project
	- Run/Build ASP.net web app project
	- Project file
	- launchSettings.json -> for running application
		- environmental variable (DEV/PROD) -> profiles
	- wwwroot folder -> static content (css/js/nuget/img)
	- appsettings.json
		- connection strings/secret key/db connection
	- Program.cs 
		- declare services
		- Configure routing
## 2. MVC project fundamentals
	- Model: Shape of data
	- View: user interface (html/css)
		- Partial view - underscore in name, shared views/used in main views
	- Default Layout -> configured in _ViewStart.cshtml
		- _Viewimports.cshtml -> general using statements
	- Controller: process/fetch data, handles user request
		- Routing: url/controller/action/id -> done by controller

	- Naming:
		- Models can change name unlike View/Controller
		- Controller class named with same name as view + suffix "Controller"
			- automatic find view with View()
	- Data annotation in Models
		- set primary/foreign key
		- required
		- helps with generating sql script

	- Entity framework core configuration
		- Dbcontext class -> get connection string from appsettings file
		- Set services in Program.cs
			- pass connection string to AddDbContext
		- Package manager
			- add-migration
				- update database based on model
				- used to keep database in sync with application model update
			- run command update-database
				- not mentioned in video: need to install Microsoft.EntityFrameworkCore.Tools package
			- generated migration script in migrations folder
		- Run update-database
			- Checks for any migration that have not been applied
				- Migrations table in DB
			- Converts code to SQL code to update db

		-Adding Category view
			- Show data from db in view
				- pass returned list to View
				- fetch list in view
					- @model and @use to bind model to view

			-Create new page (add category)
				- define category controller
					- initialize applicationdbcontext for db connection
					- create constructor
				- Add action method in controller
					- IActionResult return view
					- Index view
						- get list of categories and pass to view
					- Create
						- get form and post to controller
						- controller creates item in db
						- check model from post action using ModelState.IsValid
					- Delete
						- Gets id and item frm db and pass model item to delete view
						- Get specific item using Find()/FirstOrDefault()
					- Edit/Update
				- Helper tags in html
					- asp-for: used in forms. binds input html to model prop
					- asp-action: specify the controller action name
					- asp-controller: specify the controller for the view (not mandatory)
				- Form validation

					- Serverside validation -> uses data annotation set in model class
					- uses ModelState.IsValid to check before pushing changes to db
					- Use helper-tags in View to display error message during model validation
						- asp-validation-for: uses data annotation attribute and produce html attributes needed for jquery validation on client side
					- Custom error message with data annotation in Model
						- using ErrorMessage data annotation
					- Add custom model validation logic in Controller -> with custom error message and display with TempData
					- Model validation summary using asp-validation-sumamry: All vs ModelOnly (does not display errors related to model prop)
					- Client side validation using Js
						- Adding partial view script to view and calling the validation jquery file
					- Temp data: only lives for 1 request -> cleared on refresh
						- use partial views to display temp data 
				- Model-View
					- Passing data to controller action method with asp-route..

## 3. Razor Pages
	- No controller
	- simpler way to build page based applications
	- follows mvvm pattern -> model-view-viewmodel
	- pros:
		- more maintainable code & folder structure
		- routing is easier
		- more secure
			- antiforgerytoken validation by default
	- Pages
		- Pagemodel
			- OnGet
			- OnPost
			- Automatic binding, no need to return view
			- asp-page to define route. no asp-action
			https://www.learnrazorpages.com/razor-pages/handler-methods
		- CRUD operations
			- BindProperty tag for POST
			...
			https://www.learnrazorpages.com/razor-pages/action-results
			- calling Page() vs RedirectToPage(...)
				- Page() does not run OnGet
				- 

## 4. N-tier architecture
	- design pattern -> seperate application into layers (e.g presentation, business logic, data access)
	- associate different layers using project reference 
	- Service lifetimes in dependency injection
		- transient
			- safest
			- object created every time it is request. No reuse
			- new implementation everytime method is called
		- scoped
			- created once per http request
			- lifetime depends on requets scope
			- creatde on request
		- singlton
			- created once per app lifetime
			- stays same for life cycle of application

	- Repository Pattern with Unit Of Work
		- interface for generic CRUD operations from DB
			- interface for specific class (ICategoryRepository...)
		- Dependency injection to provide interface to controller
			- Add to service with correct lifetime 
		- Renaming project
			- Renaming project file
			- Rename namespace
			- Update css issolation styling file in _layout file
			- Update dependency
		- unit of work abstraction for global/shared method
			- https://learn.microsoft.com/en-us/aspnet/mvc/overview/older-versions/getting-started-with-ef-5-using-mvc-4/implementing-the-repository-and-unit-of-work-patterns-in-an-asp-net-mvc-application
			- in unit of work, we can access all the repositories that was implemented.
			- cons: creates implementation of all repositiories (even when not all is needed)

## 5. Areas in MVC & Interface classes
	- Seperate group of pages/routing for different functionalities (e.g admin dashboard & normal web page)
	- define _ViewImports and _ViewStart in Area/View folders
	- Update routing => view files with "asp-area"
	- Area > Controller > Action name
	-After adding new model
		- new interface class -> IProductRepository.cs
		- new interface implementation -> ProductRepository.cs
		- add new repository to UnitOfWorkds interface & implementation
	- Foreign key in model
	- Pass multiple objects to view from controller
		- ViewBag
			-> dynamic property: sets property using ViewBag.Property = Value
			-> transfer data from controller to view but not in reverse
			-> can assign any number of prop
			-> lifetime last for current http request
			-> wrapper for ViewData
		- ViewData
			- Dictionary type: ViewData[Property] = Value
			- must be type casted before use (in view)
			- lifetime only for current http request
		- TempData
			- store data between 2 consecutive request
			- uses session
	
		- ViewModel
			- models specifically designed for a view
			- strongly binded to a view
				- custom controller action

	- Get property of object associated with foreign key (Category)
		- using "include" in db context calles: _db.Products.Include(x=>x.Prop...) -> from EF Core
		- modifying Get & GetAll methods in interface to get properties based on foreign key relation 

## 6. AJAX api calls
	- set up api endpoints -> return as json
	- built in support for api in .net core