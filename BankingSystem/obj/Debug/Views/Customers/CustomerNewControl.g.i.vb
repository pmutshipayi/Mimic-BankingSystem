#ExternalChecksum ("..\..\..\..\Views\Customers\CustomerNewControl.xaml", "{8829d00f-11b8-4213-878b-770e8597ac16}", "1283C209F5A79697BDA27400B715C75BC2ED1B114C0351C27223C32E91D7832A")
'------------------------------------------------------------------------------
' <auto-generated>
'     This code was generated by a tool.
'     Runtime Version:4.0.30319.42000
'
'     Changes to this file may cause incorrect behavior and will be lost if
'     the code is regenerated.
' </auto-generated>
'------------------------------------------------------------------------------

Option Strict Off
Option Explicit On

Imports BankingSystem
Imports MahApps.Metro.IconPacks
Imports System
Imports System.Diagnostics
Imports System.Windows
Imports System.Windows.Automation
Imports System.Windows.Controls
Imports System.Windows.Controls.Primitives
Imports System.Windows.Data
Imports System.Windows.Documents
Imports System.Windows.Ink
Imports System.Windows.Input
Imports System.Windows.Markup
Imports System.Windows.Media
Imports System.Windows.Media.Animation
Imports System.Windows.Media.Effects
Imports System.Windows.Media.Imaging
Imports System.Windows.Media.Media3D
Imports System.Windows.Media.TextFormatting
Imports System.Windows.Navigation
Imports System.Windows.Shapes
Imports System.Windows.Shell


'''<summary>
'''CustomerNewControl
'''</summary>
<Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Public Class CustomerNewControl
    Inherits System.Windows.Controls.UserControl
    Implements System.Windows.Markup.IComponentConnector


#ExternalSource ("..\..\..\..\Views\Customers\CustomerNewControl.xaml", 55)
    <System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")> _
    Friend WithEvents firstName As System.Windows.Controls.TextBox

#End ExternalSource


#ExternalSource ("..\..\..\..\Views\Customers\CustomerNewControl.xaml", 56)
    <System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")> _
    Friend WithEvents lastName As System.Windows.Controls.TextBox

#End ExternalSource


#ExternalSource ("..\..\..\..\Views\Customers\CustomerNewControl.xaml", 57)
    <System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")> _
    Friend WithEvents idNumber As System.Windows.Controls.TextBox

#End ExternalSource


#ExternalSource ("..\..\..\..\Views\Customers\CustomerNewControl.xaml", 58)
    <System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")> _
    Friend WithEvents phoneNumber As System.Windows.Controls.TextBox

#End ExternalSource


#ExternalSource ("..\..\..\..\Views\Customers\CustomerNewControl.xaml", 59)
    <System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")> _
    Friend WithEvents cellNumber As System.Windows.Controls.TextBox

#End ExternalSource


#ExternalSource ("..\..\..\..\Views\Customers\CustomerNewControl.xaml", 60)
    <System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")> _
    Friend WithEvents email As System.Windows.Controls.TextBox

#End ExternalSource


#ExternalSource ("..\..\..\..\Views\Customers\CustomerNewControl.xaml", 65)
    <System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")> _
    Friend WithEvents dateOfBirth As System.Windows.Controls.DatePicker

#End ExternalSource

    Private _contentLoaded As Boolean

    '''<summary>
    '''InitializeComponent
    '''</summary>
    <System.Diagnostics.DebuggerNonUserCodeAttribute(), _
     System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")> _
    Public Sub InitializeComponent() Implements System.Windows.Markup.IComponentConnector.InitializeComponent
        If _contentLoaded Then
            Return
        End If
        _contentLoaded = true
        Dim resourceLocater As System.Uri = New System.Uri("/BankingSystem;component/views/customers/customernewcontrol.xaml", System.UriKind.Relative)

#ExternalSource ("..\..\..\..\Views\Customers\CustomerNewControl.xaml", 1)
        System.Windows.Application.LoadComponent(Me, resourceLocater)

#End ExternalSource
    End Sub

    <System.Diagnostics.DebuggerNonUserCodeAttribute(), _
     System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0"), _
     System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never), _
     System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes"), _
     System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity"), _
     System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")> _
    Sub System_Windows_Markup_IComponentConnector_Connect(ByVal connectionId As Integer, ByVal target As Object) Implements System.Windows.Markup.IComponentConnector.Connect
        If (connectionId = 1) Then
            Me.firstName = CType(target, System.Windows.Controls.TextBox)
            Return
        End If
        If (connectionId = 2) Then
            Me.lastName = CType(target, System.Windows.Controls.TextBox)
            Return
        End If
        If (connectionId = 3) Then
            Me.idNumber = CType(target, System.Windows.Controls.TextBox)
            Return
        End If
        If (connectionId = 4) Then
            Me.phoneNumber = CType(target, System.Windows.Controls.TextBox)
            Return
        End If
        If (connectionId = 5) Then
            Me.cellNumber = CType(target, System.Windows.Controls.TextBox)
            Return
        End If
        If (connectionId = 6) Then
            Me.email = CType(target, System.Windows.Controls.TextBox)
            Return
        End If
        If (connectionId = 7) Then
            Me.dateOfBirth = CType(target, System.Windows.Controls.DatePicker)
            Return
        End If
        If (connectionId = 8) Then
            Me.physicalAddress = CType(target, System.Windows.Controls.RichTextBox)
            Return
        End If
        Me._contentLoaded = true
    End Sub

    Friend WithEvents physicalAddress As System.Windows.Controls.TextBox
End Class

