Imports System
Imports System.Drawing
Imports System.Diagnostics
Imports System.Windows.Forms

Public Class DataGridValidatedTextColumn
    Inherits DataGridTextBoxColumn

#Region "Declaraciones"

    Private m_controlSize As Size
    Private m_padding As DataGridColumnStylePadding

    Public Event Cambio_Valor(ByRef p_ctrlTextBox As DataGridTextBox)

#End Region

#Region "Propiedades"

    Public Property Padding() As DataGridColumnStylePadding
        Get
            Return m_padding
        End Get
        Set(ByVal Value As DataGridColumnStylePadding)
            m_padding = Value
        End Set
    End Property

    Public Property ControlSize() As Size
        Get
            Return m_controlSize
        End Get
        Set(ByVal Value As Size)
            m_controlSize = Value
        End Set
    End Property


#End Region

#Region "Constructor"

    Public Sub New()
        Me.Padding = New DataGridColumnStylePadding(0)
        Me.ControlSize = New Size(200, 24)
        Me.Width = Me.GetPreferredSize(Nothing, Nothing).Width
    End Sub 'New 

#End Region

#Region "Procedimientos"

    'Protected Overloads Overrides Sub Paint(ByVal g As System.Drawing.Graphics, ByVal bounds As System.Drawing.Rectangle, ByVal [source] As CurrencyManager, ByVal rowNum As Integer, ByVal backBrush As System.Drawing.Brush, ByVal foreBrush As System.Drawing.Brush, ByVal alignToRight As Boolean)
    '    Dim objFont As Font
    '    Dim objFontFirst As Font
    '    Dim strTexto As String

    '    Try
    '        Dim sf As New StringFormat

    '        sf.Alignment = StringAlignment.Near
    '        sf.LineAlignment = StringAlignment.Center

    '        If rowNum = 0 Then
    '            g.FillRectangle(Brushes.White, bounds)
    '            g.DrawRectangle(New Pen(Color.FromArgb(77, 77, 77), 1), New Rectangle(bounds.X, bounds.Y, bounds.Width, bounds.Height - 1))
    '        Else
    '            g.FillRectangle(backBrush, bounds)
    '        End If

    '        Dim boundsF As New System.Drawing.RectangleF( _
    '            CType(bounds.X, Single), _
    '            CType(bounds.Y, Single), _
    '            CType(bounds.Width, Single), _
    '            CType(bounds.Height, Single))


    '        objFont = Me.DataGridTableStyle.DataGrid.Font
    '        objFontFirst = New Font(objFont, FontStyle.Regular)

    '        If Me.GetColumnValueAtRow([source], rowNum) Is DBNull.Value Then
    '            strTexto = ""
    '        Else
    '            strTexto = Me.GetColumnValueAtRow([source], rowNum)
    '        End If

    '        'If IsNumeric(DirectCast(strTexto, String)) Then
    '        '    strTexto = Microsoft.VisualBasic.Format(DirectCast(strTexto, Double), m_Format)
    '        'ElseIf IsDate(DirectCast(strTexto, String)) Then
    '        '    strTexto = Microsoft.VisualBasic.Format(DirectCast(strTexto, Date), m_Format)
    '        'End If

    '        If rowNum = 0 Then
    '            g.DrawString(strTexto, objFontFirst, foreBrush, boundsF, sf)
    '        Else
    '            g.DrawString(strTexto, objFont, foreBrush, boundsF, sf)
    '        End If
    '    Catch ex As Exception
    '        MsgBox(ex.Message)
    '    End Try
    'End Sub

    'Protected Overrides Function Commit(ByVal dataSource As CurrencyManager, ByVal rowNum As Integer) As Boolean
    '    Dim blnValueCambia As Boolean = False

    '    Try

    '        If rowNum = 0 Then
    '            If Me.TextBox.Text = "" Then

    '                If Not Me.GetColumnValueAtRow(dataSource, rowNum) Is DBNull.Value Then
    '                    blnValueCambia = True
    '                End If

    '                Me.SetColumnValueAtRow(dataSource, rowNum, DBNull.Value)
    '            Else

    '                'If Not Me.GetColumnValueAtRow(dataSource, rowNum) Is DBNull.Value Then
    '                '    If Me.GetColumnValueAtRow(dataSource, rowNum) = Me.TextBox.Text Then
    '                '        blnValueCambia = True
    '                '    End If
    '                'Else
    '                '    blnValueCambia = True
    '                'End If

    '                blnValueCambia = True

    '                Me.SetColumnValueAtRow(dataSource, rowNum, Me.TextBox.Text)
    '            End If
    '            Me.HideEditBox()
    '        End If


    '        If blnValueCambia Then
    '            RaiseEvent Cambio_Valor()
    '        End If

    '        Return True

    '    Catch ex As Exception
    '        Return False
    '    End Try

    'End Function

    'Protected Overloads Overrides Sub Edit(ByVal [source] As CurrencyManager, ByVal rowNum As Integer, ByVal bounds As Rectangle, ByVal [readOnly] As Boolean, ByVal instantText As String, ByVal cellIsVisible As Boolean)
    '    If rowNum = 0 Then

    '        bounds.Width -= 1
    '        bounds.Height -= 1
    '        MyBase.Edit([source], rowNum, bounds, [readOnly], instantText, cellIsVisible)

    '    End If
    'End Sub

    Protected Overrides Function GetMinimumHeight() As Integer
        Return GetPreferredHeight(Nothing, Nothing)
    End Function

    Protected Overrides Function GetPreferredHeight(ByVal g As System.Drawing.Graphics, ByVal value As Object) As Integer
        Return Me.ControlSize.Height + Me.Padding.Top + Me.Padding.Bottom
    End Function

    Protected Overrides Function GetPreferredSize(ByVal g As System.Drawing.Graphics, ByVal value As Object) As System.Drawing.Size

        Dim width As Integer = Me.ControlSize.Width + Me.Padding.Left + Me.Padding.Right
        Dim height As Integer = Me.ControlSize.Height + Me.Padding.Top + Me.Padding.Bottom

        Return New Size(width, height)
    End Function

    Protected Overloads Overrides Sub Paint(ByVal g As System.Drawing.Graphics, ByVal bounds As System.Drawing.Rectangle, ByVal [source] As CurrencyManager, ByVal rowNum As Integer)
        Me.Paint(g, bounds, [source], rowNum, False)
    End Sub

    Protected Overloads Overrides Sub Paint(ByVal g As System.Drawing.Graphics, ByVal bounds As System.Drawing.Rectangle, ByVal [source] As CurrencyManager, ByVal rowNum As Integer, ByVal alignToRight As Boolean)
        Me.Paint(g, bounds, [source], rowNum, Brushes.White, Brushes.Black, False)
    End Sub

    Protected Overrides Sub SetColumnValueAtRow(ByVal source As System.Windows.Forms.CurrencyManager, ByVal rowNum As Integer, ByVal value As Object)
        MyBase.SetColumnValueAtRow(source, rowNum, value)
        RaiseEvent Cambio_Valor(Me.TextBox)
    End Sub

    Public Shadows Property Format() As String
        Get
            Return MyBase.Format
        End Get
        Set(ByVal value As String)
            MyBase.Format = value
        End Set
    End Property


#End Region

End Class
