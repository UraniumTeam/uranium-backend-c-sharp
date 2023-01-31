# Uranium Backend Library
Is a library for profiling C# applications.

# Usage
1. Add Library to your project
2. In the beginning of profiling functions insert this code
```Csharp
public static void SomeFunction()
{
    using var scope = ProfilerScope.Begin(nameof(SomeFunction));
    //Some code
}
```
3. In Main function after all profiling function calls insert this code

```Csharp
public static void Main()
{
    //Some code
    SomeFunction();
    ProfilerInstance.Save();
}
```
4. Build your application
5. After this manipulations in current directory will create `.UPS` file
6. To visualize call stacks you can use [Uranium Studio](https://github.com/UraniumTeam/UraniumStudio)
