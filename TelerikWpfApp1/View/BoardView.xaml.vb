Imports System.Windows.Ink

Public Class BoardView
    Inherits UserControl
    Public scale As Double = 1
    Public gridmove As Boolean
    Public gridmovep0 As Point
    Public gridmoveorip As Point

    Public s As List(Of StrokeCollection)
    Public n As Int32
    Public g1, g2, tmp As Grid
    Public Sub New()

        ' 此调用是设计器所必需的。
        InitializeComponent()
        InkCanvas1.EraserShape = New Ink.RectangleStylusShape(30, 50)
        ' 在 InitializeComponent() 调用之后添加任何初始化。
        g1 = c1
        g2 = c2
        s = New List(Of StrokeCollection)
        s.Add(New StrokeCollection())
        n = 0
    End Sub

    Public Sub ChangePage(x As StrokeCollection)
        trans.Source = RenderVisualAsImage(Grid1)
        g1.Children.Clear()
        g2.Children.Clear()
        g1.Children.Add(trans)
        g2.Children.Add(InkCanvas1)
        InkCanvas1.Strokes = x
        Transitioner.SelectedIndex = 1 - Transitioner.SelectedIndex
        tmp = g1
        g1 = g2
        g2 = tmp
    End Sub

    Public Sub AddPage()
        s(n) = InkCanvas1.Strokes
        s.Add(New StrokeCollection)
        n = s.Count - 1
        'cv.InkCanvas1.Strokes = s(n)
        ChangePage(s(n))
    End Sub

    Public Sub PrevPage()
        If n = 0 Then
            Exit Sub
        End If
        s(n) = InkCanvas1.Strokes
        n = n - 1
        'cv.InkCanvas1.Strokes = s(n)
        ChangePage(s(n))
    End Sub

    Public Sub NextPage()
        If n = s.Count - 1 Then
            Exit Sub
        End If
        s(n) = InkCanvas1.Strokes
        n = n + 1
        'cv.InkCanvas1.Strokes = s(n)
        ChangePage(s(n))
    End Sub

    Public Function getlabel() As String
        Return CStr(n + 1) + "/" + CStr(s.Count)
    End Function

#Region "Grid Scale&Move"

    Private Sub Grid1_PreviewMouseWheel(ByVal sender As Object, ByVal e As MouseWheelEventArgs) Handles Grid1.PreviewMouseWheel
        Dim scale1 As Double
        If e.Delta < 0 Then
            scale1 = scale * 0.9
        Else
            scale1 = scale * 1.1
        End If
        Dim st As ScaleTransform = FindScaleTransform(Grid1.LayoutTransform)
        st.ScaleX = scale1
        st.ScaleY = scale1
        scale = scale1
    End Sub

    Private Sub InkCanvas1_SizeChanged(sender As Object, e As SizeChangedEventArgs) Handles InkCanvas1.SizeChanged
        If e.NewSize.Width <> MyBackControl.ActualWidth Then
            InkCanvas1.Width = MyBackControl.ActualWidth
        End If
        If e.NewSize.Height <> MyBackControl.ActualHeight Then
            InkCanvas1.Height = MyBackControl.ActualHeight
        End If
    End Sub

    Private Sub Grid1_MouseMove(sender As Object, e As MouseEventArgs) Handles Grid1.MouseMove
        'Console.WriteLine("MainGrid_MouseMove")
        If gridmove Then
            Dim p1 As Point = Mouse.GetPosition(Me)
            Canvas.SetLeft(Grid1, gridmoveorip.X + p1.X - gridmovep0.X)
            Canvas.SetTop(Grid1, gridmoveorip.Y + p1.Y - gridmovep0.Y)
        End If
    End Sub

    Private Sub Grid1_MouseDown(sender As Object, e As MouseButtonEventArgs) Handles Grid1.MouseDown
        'Console.WriteLine("MainGrid_MouseDown")
        gridmove = True
        gridmovep0 = Mouse.GetPosition(Me)
        gridmoveorip = New Point(Canvas.GetLeft(Grid1), Canvas.GetTop(Grid1))
    End Sub

    Private Sub Grid1_MouseUp(sender As Object, e As MouseButtonEventArgs) Handles Grid1.MouseUp
        'Console.WriteLine("MainGrid_MouseUp")
        gridmove = False
    End Sub

    Private Sub MainGrid_MouseLeave(sender As Object, e As MouseEventArgs) Handles Grid1.MouseLeave
        'Console.WriteLine("MainGrid_MouseLeave")
        gridmove = False
    End Sub


#End Region

End Class
