cmake_minimum_required(VERSION 3.24)
project("reMind" CSharp)
include(CSharpUtilities)
#add_executable("reMind"
#${PROJECT_SOURCE_DIR}/Program.cs
#${PROJECT_SOURCE_DIR}/DateManager.cs
#${PROJECT_SOURCE_DIR}/FlatFileManager.cs
#${PROJECT_SOURCE_DIR}/InferfaceManager.cs
#)
add_executable(main Program.cs)
#set_property(TARGET reMind PROPERTY VS_DOTNET_REFERENCES
#      "Microsoft.CSharp"
#      "PresentationCore"
#      "PresentationFramework"
#      "System"
#      "System.Core"
#      "System.Data"
#      "System.Data.DataSetExtensions"
#      "System.Net.Http"
#      "System.Xaml"
#      "System.Xml"
#      "System.Xml.Linq"
#      "WindowsBase")