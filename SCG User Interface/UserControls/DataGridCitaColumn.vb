Public Class DataGridCitaColumn
    Inherits DataGridColumnStyle
    Private m_controlSize As Size

    Private m_padding As DataGridColumnStylePadding

    Private m_FuenteNegrita As Boolean

    Private m_Format As String = ""

    Public Sub New()
        Me.Padding = New DataGridColumnStylePadding(0)
        Me.ControlSize = New Size(200, 24)
        Me.Width = Me.GetPreferredSize(Nothing, Nothing).Width
    End Sub 'New 

    Public Property Format() As String
        Get
            Return m_Format
        End Get
        Set(ByVal Value As String)
            m_Format = Value
        End Set
    End Property

    Public Property scgFuenteNegrita() As Boolean
        Get
            Return m_FuenteNegrita
        End Get
        Set(ByVal Value As Boolean)
            m_FuenteNegrita = Value
        End Set
    End Property

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

    Protected Overloads Overrides Sub Paint(ByVal g As System.Drawing.Graphics, ByVal bounds As System.Drawing.Rectangle, ByVal [source] As CurrencyManager, ByVal rowNum As Integer, ByVal backBrush As System.Drawing.Brush, ByVal foreBrush As System.Drawing.Brush, ByVal alignToRight As Boolean)
        Dim objFont As Font
        Dim strTexto As String

        Try
            Dim sf As New StringFormat

            sf.Alignment = StringAlignment.Near
            sf.LineAlignment = StringAlignment.Center
            g.FillRectangle(backBrush, bounds)

            'objFont = Me.DataGridTableStyle.DataGrid.Font
            objFont = New Font(Me.DataGridTableStyle.DataGrid.Font, FontStyle.Bold)

            If Me.GetColumnValueAtRow([source], rowNum) Is DBNull.Value Then
                strTexto = ""
            Else
                strTexto = Me.GetColumnValueAtRow([source], rowNum)
            End If

            If strTexto = "" Then

                g.FillRectangle(New SolidBrush(Color.White), bounds)

            Else

                g.FillRectangle(New SolidBrush(Color.FromArgb(156, 186, 206)), bounds)

            End If

            If strTexto <> "" Then

                strTexto = GetConcatOTs(strTexto)

            End If

            Dim boundsR As New System.Drawing.Rectangle( _
                CType(bounds.X, Single), _
                CType(bounds.Y + 1, Single), _
                CType(bounds.Width - 1, Single), _
                CType(bounds.Height - 2, Single))

            g.DrawRectangle(New Pen(Color.Black, 1), boundsR)

            g.DrawString(strTexto, objFont, foreBrush, boundsR, sf)

        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    Protected Overrides Sub Abort(ByVal rowNum As Integer) '
        ' no implementation 
    End Sub 'Abort

    Protected Overrides Function Commit(ByVal dataSource As CurrencyManager, ByVal rowNum As Integer) As Boolean
        Return True
    End Function 'Commit

    Protected Overloads Overrides Sub Edit(ByVal [source] As CurrencyManager, ByVal rowNum As Integer, ByVal bounds As Rectangle, ByVal [readOnly] As Boolean, ByVal instantText As String, ByVal cellIsVisible As Boolean) ' no implementation 
    End Sub 'Edit

    Protected Overrides Function GetMinimumHeight() As Integer
        Return GetPreferredHeight(Nothing, Nothing)
    End Function 'GetMinimumHeight

    Protected Overrides Function GetPreferredHeight(ByVal g As System.Drawing.Graphics, ByVal value As Object) As Integer
        Return Me.ControlSize.Height + Me.Padding.Top + Me.Padding.Bottom
    End Function 'GetPreferredHeight

    Protected Overrides Function GetPreferredSize(ByVal g As System.Drawing.Graphics, ByVal value As Object) As System.Drawing.Size

        Dim width As Integer = Me.ControlSize.Width + Me.Padding.Left + Me.Padding.Right
        Dim height As Integer = Me.ControlSize.Height + Me.Padding.Top + Me.Padding.Bottom

        Return New Size(width, height)
    End Function 'GetPreferredSize

    Protected Overloads Overrides Sub Paint(ByVal g As System.Drawing.Graphics, ByVal bounds As System.Drawing.Rectangle, ByVal [source] As CurrencyManager, ByVal rowNum As Integer)
        Me.Paint(g, bounds, [source], rowNum, False)
    End Sub 'Paint

    Protected Overloads Overrides Sub Paint(ByVal g As System.Drawing.Graphics, ByVal bounds As System.Drawing.Rectangle, ByVal [source] As CurrencyManager, ByVal rowNum As Integer, ByVal alignToRight As Boolean)
        Me.Paint(g, bounds, [source], rowNum, Brushes.White, Brushes.Black, False)
    End Sub 'Paint

    Private Function GetConcatOTs(ByVal p_strTexto As String) As String
        Dim intCont1 As Integer
        Dim strTemp As String = ""
        Dim strResult As String = ""

        For intCont1 = 0 To p_strTexto.Split(Chr(13)).Length - 1

            strTemp = p_strTexto.Split(Chr(13))(intCont1)

            strTemp = strTemp.Split(" ")(1)

            strResult = strResult & IIf(strResult = "", "", ",") & strTemp

        Next

        Return strResult

    End Function

End Class
