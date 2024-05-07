using System.Reflection;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Win32;
using Figure = GeometryLibrary.Figure;
using Point = GeometryLibrary.Point;
using Line = GeometryLibrary.Line;
using Ellipse = GeometryLibrary.Ellipse;
using Rectangle = GeometryLibrary.Rectangle;

namespace Task3;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{

    private Type? selectedType;

    private object createdInstance;

    public MainWindow()
    {
        InitializeComponent();
    }

    private void BrowseButton_Click(object sender, RoutedEventArgs e)
    {
        OpenFileDialog openFileDialog = new OpenFileDialog();
        openFileDialog.Filter = "DLL Files (*.dll)|*.dll";
        if(openFileDialog.ShowDialog() == true)
        {
            string assemblyName = openFileDialog.FileName;
            var assembly = Assembly.LoadFile(assemblyName);
            Type abstractClassType = assembly.GetType("GeometryLibrary.Figure");
            Type[] implementedClasses = GetImplementedClasses(assembly.GetTypes(), abstractClassType);

            FillComboBox(implementedClasses);
        }
    }
    
    private void FillComboBox(Type[] types)
    {
        ClassListBox.ItemsSource = types.Select(t => t.FullName);
    }
    
    private Type[] GetImplementedClasses(Type[] types, Type subtype)
    {
        return types.Where(t => t.IsClass)
            .Where(t => t.IsSubclassOf(subtype))
            .Where(t => t.IsAbstract == false).ToArray();
    }

    private void ClassListBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
    {
        string? selectedClassName = ClassListBox.SelectedItem as string;
        if (!string.IsNullOrEmpty(selectedClassName))
        {
            Assembly assem = typeof(Figure).Assembly;
            selectedType = assem.GetType(selectedClassName);
            ConstructorInfo[] constructors = selectedType.GetConstructors();
            ConstructorStackPanel.Children.Clear();
            foreach (ConstructorInfo constructor in constructors)
            {
                StackPanel panel = new StackPanel();
                panel.Orientation = Orientation.Horizontal;
                panel.Margin = new Thickness(0, 5, 0, 5);

                ParameterInfo[] parameters = constructor.GetParameters();
                foreach (ParameterInfo param in parameters)
                {
                    TextBox textBox = new TextBox();
                    textBox.Margin = new Thickness(5, 0, 0, 0);
                    textBox.Tag = param.ParameterType;
                    textBox.Text = param.Name;
                    panel.Children.Add(textBox);
                }

                ConstructorStackPanel.Children.Add(panel);
            }
            ExecuteConstructorButton.IsEnabled = true;
        }
    }
    
    private void ExecuteConstructorButton_Click(object sender, RoutedEventArgs e)
    {
        if (selectedType != null)
        {
            try
            {
                List<object> parameters = new List<object>();
                foreach (StackPanel panel in ConstructorStackPanel.Children)
                {
                    foreach (TextBox textBox in panel.Children)
                    {
                        object value = Convert.ChangeType(textBox.Text, (Type)textBox.Tag);
                        parameters.Add(value);
                    }
                }
                createdInstance = Activator.CreateInstance(selectedType, parameters.ToArray());
                
                MethodInfo[] methods = selectedType.GetMethods(BindingFlags.Public | BindingFlags.Instance);
                MethodComboBox.ItemsSource = methods;
                ExecuteMethodButton.IsEnabled = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error creating instance: " + ex.Message);
            }
        }
    }
    
    private void MethodComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        MethodInfo selectedMethod = (MethodInfo)MethodComboBox.SelectedItem;
        if (selectedMethod != null)
        {
            ParameterInfo[] parameters = selectedMethod.GetParameters();
            MethodParameterStackPanel.Children.Clear();
            foreach (ParameterInfo param in parameters)
            {
                TextBox textBox = new TextBox();
                textBox.Margin = new Thickness(0, 5, 0, 5);
                textBox.Tag = param.ParameterType;
                textBox.Text = param.Name;
                MethodParameterStackPanel.Children.Add(textBox);
            }
            ExecuteMethodButton.IsEnabled = true;
        }
    }
    
    private void ExecuteMethodButton_Click(object sender, RoutedEventArgs e)
    {
        MethodInfo selectedMethod = (MethodInfo)MethodComboBox.SelectedItem;
        if (selectedMethod != null)
        {
            try
            {
                List<object> parameters = new List<object>();
                foreach (TextBox textBox in MethodParameterStackPanel.Children)
                {
                    object value = Convert.ChangeType(textBox.Text, (Type)textBox.Tag);
                    parameters.Add(value);
                }

                object result = selectedMethod.Invoke(createdInstance, parameters.ToArray());
                MessageBox.Show("Method execution result: " + result.ToString());
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error executing method: " + ex.Message);
            }
        }
    }
}