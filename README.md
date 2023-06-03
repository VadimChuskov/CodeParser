Install-Package Microsoft.EntityFrameworkCore.Tools

Update-Package Microsoft.EntityFrameworkCore.Tools

Add-Migration InitialCreate
>No DbContext was found in assembly 'CodeParser'. Ensure that you're using the correct assembly and that the type is neither abstract nor generic.

! Default project: CodeParser.Database
Add-Migration InitialCreate
>Could not load assembly 'CodeParser.Database'. Ensure it is referenced by the startup project 'CodeParser'.

Remove-Migration

Update-Database