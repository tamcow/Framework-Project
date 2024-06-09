Scaffolding has generated all the files and added the required dependencies.

However the Application's Startup code may require additional changes for things to work end to end.
Add the following code to the Configure method in your Application's Startup class if not already done:

        app.UseEndpoints(endpoints =>
        {
          endpoints.MapControllerRoute(
            name : "areas",
            pattern : "{area:exists}/{controller=Home}/{action=Index}/{id?}"
          );
        });

 Scaffold-DbContext "Server=DESKTOP-TTM610I;Database=dbFrame;Integrated Security=true;" Microsoft.EntityFrameworkCore.SqlServer -OutputDir Models -f
DESKTOP-TTM610I
.\SQLEXPRESS