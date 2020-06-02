SWEN90013 Hospital Scheduling
===============================

Nurse bed allocation solution for ICU at Royal Melbourne Hospital, this project
seeks to provide the following components:

  * A pod nurse-bed allocation database
  * Automatic allocation seeding, to provide a sane starting point for manual
    allocation
  * Web service to deliver allocation data
  * Web UI (for use in IE9/10) for allocation management
  * Web panel for allocation display

Dependencies
---

To run the project, the following dependencies are required:

  * [.NET Core CLI](https://www.microsoft.com/net/core)
  * [Angular 2 CLI](https://cli.angular.io/)
  * [NodeJS local-web-server](https://www.npmjs.com/package/local-web-server)

Follow the instructions to install .NET Core. To install the Node dependencies perform the following.

```shell
npm install -g @angular/cli
npm install -g local-web-server
```

Running the Project
---

The project is available as a cross-platform build on both
UNIX-based systems and Windows.

To run on Windows, use the `build.psm1` module:

```powershell
Import-Module ./build.psm1
Start-Application
```

To run on UNIX, just execute the makefile with:

```shell
make
```

Running with mock server
------------------------

To run the application with mock API backend:

```shell
make run_mock
```

NB : Running mock server does not build the project to save time. Run `make build_dev` before
`make run_mock` if you so desire.


Building the production release
-------------------------------

To make the release build, run `make build_prod`.

By default this will put the release in `production_build`, but can be set like
this: `PROD_DIR="~/Desktop/other_file" make build_prod`.

Be wary that this will remove `$PROD_DIR` before running.


Database Commands
------------------------
The following make targets exist to make database creation easier:
* `make make_database` in order to create the migration and update the database.
* `make nuke_database` in order to remove the migrations and the database.
* `make remake_database` in order to remove any existing database and migrations and create the database again.

In order to create the database and migrations manually, run:
```shell
cd to $PROJECT_ROOT/dotnet-server/HospitalAllocation
run  dotnet restore
run  dotnet ef migrations add HospitalAllocation
run  dotnet ef database update
```

Killing processes that hog ports
--------------------------------

Because some of the node infrastructure we use is not good at shutting down
nicely, sometimes you might need to kill processes using the ports we need.

You can do this easily with `make kill_ports`.

Be wary that this will kill any process on any of the ports this project uses.

Test
---

To run the tests, run:

```shell
make test
```

The Team
--------

* [Saksham Agrawal](mailto:sakshama@student.unimelb.edu.au) `sakshama`
* [Arwinder Singh](mailto:arwinders@student.unimelb.edu.au) `arwinders`
* [David Barrell](mailto:dbarrell@student.unimelb.edu.au) `dbarrell`
* [Zhenting Wu](mailto:zhentingw@student.unimelb.edu.au) `zhentingw`
* [Hao Mai](mailto:hmai1@student.unimelb.edu.au) `hmai1`
* [Qianglin Kong](mailto:qianglink@student.unimelb.edu.au) `qianglink`
* [Jianbo Ma](mailto:jianbom@student.unimelb.edu.au) `jianbom`
* [Yang Xiong](mailto:xiong1@student.unimelbe.edu.au) `xiong1`
* [Weixu Chen](mailto:weixuc@student.unimelb.edu.au) `weixuc`
* [Haris Rafique Khan](mailto:harisk@student.unimelb.edu.au) `harisk`
* [Sirui Yan](mailto:siruiy@student.unimelb.edu.au) `siruiy`

Previous Team
--------

* [Bishal Sapkota](mailto:sapkotab@student.unimelb.edu.au) `sapkotab`
* [Angus Huang](mailto:angush@student.unimelb.edu.au) `angush`
* [Robert Holt](mailto:rholt@student.unimelb.edu.au) `rholt`
* [Leighann Hangfang Li](mailto:hangfanl@student.unimelb.edu.au) `hangfanl`
* [Ted Feifan Zhang](mailto:feifanz@student.unimelb.edu.au) `feifanz`
* [Boyang Xing](mailto:bxing@student.unimelb.edu.au) `bxing`
* [Jane Shuang Qiu](mailto:shuangq1@student.unimelb.edu.au) `shuangq1`
* [Jason Jincheng Shi](mailto:jinchengs@student.unimelb.edu.au) `jinchengs`
* [Leon Drygala](mailto:ldrygala@student.unimelb.edu.au) `ldrygala`
* [Jim Shiyu Jin](mailto:shiyuj@student.unimelb.edu.au) `shiyuj`