Imports System
Imports System.Drawing
Imports System.Diagnostics
Imports System.Windows.Forms
Imports DMSOneFramework


Public Class DataGridConditionalColumn
    Inherits DataGridColumnStyle
    Private m_controlSize As Size
    Private m_padding As DataGridColumnStylePadding
    Private m_ColumnaCondicional As Integer
    Private m_ColorCondicionalFijo As Color
    Private m_FuenteCondicionalFijo As Font
    Private m_ColorCondicionalCustom As Color
    Private m_FuenteCondicionalCustom As Font
    Private m_TipoColabora As Boolean = False
    '    Private m_formato As String = Nothing

    Public Sub New()
        m_ColorCondicionalCustom = Color.FromArgb(0, 0, 0)
        Me.Padding = New DataGridColumnStylePadding(0)
        Me.ControlSize = New Size(200, 24)
        Me.Width = Me.GetPreferredSize(Nothing, Nothing).Width
    End Sub 'New 

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

    Public Property P_ColumnaCondicional() As Integer
        Get
            Return m_ColumnaCondicional
        End Get
        Set(ByVal Value As Integer)
            m_ColumnaCondicional = Value
        End Set
    End Property

    Public Property P_ColorCondicional() As Color
        Get
            Return m_ColorCondicionalCustom
        End Get
        Set(ByVal Value As Color)
            m_ColorCondicionalCustom = Value
        End Set
    End Property

    Public Property P_FuenteCondicional() As Font
        Get
            Return m_FuenteCondicionalCustom
        End Get
        Set(ByVal Value As Font)
            m_FuenteCondicionalCustom = Value
        End Set
    End Property

    Public Property P_TipoColabora() As Boolean
        Get
            Return m_TipoColabora
        End Get
        Set(ByVal value As Boolean)
            m_TipoColabora = value
        End Set
    End Property

'    Public Property P_Formato() As String
'        Get
'            Return m_formato
'        End Get
'        Set(ByVal value As String)
'            Me.m_formato = value
'        End Set
'    End Property

    Protected Overloads Overrides Sub Paint(ByVal g As System.Drawing.Graphics, ByVal bounds As System.Drawing.Rectangle, ByVal [source] As CurrencyManager, ByVal rowNum As Integer, ByVal backBrush As System.Drawing.Brush, ByVal foreBrush As System.Drawing.Brush, ByVal alignToRight As Boolean)
        Dim objColStyle As DataGridConditionalColumn
        Dim objFont As Font
        Dim strTexto As String

        Dim sf As New StringFormat

        m_ColorCondicionalFijo = Me.DataGridTableStyle.ForeColor
        m_FuenteCondicionalFijo = Me.DataGridTableStyle.DataGrid.Font

        sf.Alignment = StringAlignment.Near
        sf.LineAlignment = StringAlignment.Center
        'sf.FormatFlags = StringFormatFlags.DirectionRightToLeft Or StringFormatFlags.FitBlackBox
        g.FillRectangle(backBrush, bounds)

        Dim boundsF As New System.Drawing.RectangleF( _
            CType(bounds.X, Single), _
            CType(bounds.Y, Single), _
            CType(bounds.Width, Single), _
            CType(bounds.Height, Single))

        objColStyle = Me.DataGridTableStyle.GridColumnStyles(m_ColumnaCondicional)

        If objColStyle.GetColumnValueAtRow([source], rowNum) = 0 Then
            foreBrush = New SolidBrush(m_ColorCondicionalFijo)
            objFont = m_FuenteCondicionalFijo
        Else
            If Not IsNothing(m_ColorCondicionalCustom) Then
                foreBrush = New SolidBrush(m_ColorCondicionalCustom)
            Else
                foreBrush = New SolidBrush(m_ColorCondicionalFijo)
            End If
            If Not IsNothing(m_FuenteCondicionalCustom) Then
                objFont = m_FuenteCondicionalCustom
            Else
                objFont = m_FuenteCondicionalFijo
            End If
        End If

        If Me.GetColumnValueAtRow([source], rowNum) Is DBNull.Value Then
            strTexto = ""
        Else
            strTexto = Me.GetColumnValueAtRow([source], rowNum)
        End If

        If m_TipoColabora Then
            If strTexto.Split(Chr(13)).Length > 1 Then
                'strTexto = "Varios"
                strTexto = My.Resources.ResourceUI.Varios
            End If
        End If
'        If (m_formato IsNot Nothing) Then
'            strTexto = String.Format(m_formato, DateTime.Parse(strTexto, ""))
'        End If
        g.DrawString(strTexto, objFont, foreBrush, boundsF, sf)

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

#Region "Procedimientos"

'    Private Function GetColaboraName(ByVal p_strEmpID As String) As String
'        Dim objUtilitarios As New Utilitarios(DAConexion.ConnectionString)
'        Dim strReturn As String
'
'        If p_strEmpID <> "" And IsNumeric(p_strEmpID) Then
'            strReturn = objUtilitarios.GetEmpNombre(CInt(p_strEmpID))
'        Else
'            strReturn = ""
'        End If
'
'    End Function

#End Region

End Class
