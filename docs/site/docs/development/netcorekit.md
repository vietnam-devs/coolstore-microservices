# Introduction

All .NET microservices are developed by using [NetCoreKit](https://github.com/cloudnative-netcore/netcorekit) library. So we need to make it as a `submodule` in `coolstore-microservices` project.

# Add submodule

Run the command at the root of `coolstore-microservices` project as following

```
> git submodule add https://github.com/cloudnative-netcore/netcorekit src/netcorekit
```

It should create a file `.gitmodules` with the content as below

```
[submodule "src/netcorekit"]
	path = src/netcorekit
	url = https://github.com/cloudnative-netcore/netcorekit
	ignore = dirty
```

# Update submodule

To update the content from `NetCoreKit` project, run

```
> git submodule foreach git pull origin master
```

Reference at https://stackoverflow.com/questions/5828324/update-git-submodule-to-latest-commit-on-origin
