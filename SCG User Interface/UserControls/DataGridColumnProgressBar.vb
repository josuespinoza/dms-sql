Public Class DataGridColumnProgressBar
    Inherits DataGridTextBoxColumn

#Region "Declaraciones"

    Friend WithEvents imgListBarras As System.Windows.Forms.ImageList

    Private components As System.ComponentModel.IContainer

    Private m_intLimiteVerde As Integer
    Private m_intLimiteAmarillo As Integer

    Private m_blnMostrarValor As Boolean
    Private m_blnMostrarExtremos As Boolean
    Private m_blnNegritaValor As Boolean
    Private m_blnNegritaExtremos As Boolean
    Private m_blnAllowEdit As Boolean

#End Region

#Region "Constructor"

    Public Sub New()
        InitializeComponent()
    End Sub

#End Region

#Region "Procedimientos"

    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(DataGridColumnProgressBar))
        Me.imgListBarras = New System.Windows.Forms.ImageList(Me.components)
        '
        'imgListBarras
        '
        Me.imgListBarras.ColorDepth = System.Windows.Forms.ColorDepth.Depth16Bit
        Me.imgListBarras.ImageSize = New System.Drawing.Size(32, 32)
        Me.imgListBarras.ImageStream = CType(resources.GetObject("imgListBarras.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.imgListBarras.TransparentColor = System.Drawing.Color.Transparent

    End Sub

    Protected Overloads Overrides Sub Paint(ByVal g As System.Drawing.Graphics, ByVal bounds As System.Drawing.Rectangle, ByVal source As System.Windows.Forms.CurrencyManager, ByVal rowNum As Integer, ByVal backBrush As System.Drawing.Brush, ByVal foreBrush As System.Drawing.Brush, ByVal alignToRight As Boolean)
        Dim boundsf As New Rectangle(CType(bounds.X, Single), CType(bounds.Y, Single), CType(bounds.Width, Single), CType(bounds.Height, Single))
        Dim boundsreduc As New Rectangle(CType(bounds.X + 2, Single), CType(bounds.Y + 2, Single), CType(bounds.Width - 4, Single), CType(bounds.Height - 4, Single))
        Dim FormatTexto As New StringFormat
        Dim objFontValor As Font
        Dim objFontExtremos As Font
        Dim strValor As String
        Dim decValor As Decimal

        If Me.GetColumnValueAtRow(source, rowNum) Is DBNull.Value Then
            strValor = "0%"
            decValor = 0
        Else
            strValor = CStr(Me.GetColumnValueAtRow(source, rowNum)).Trim & "%"
            decValor = CDec(Me.GetColumnValueAtRow(source, rowNum))
        End If

        g.FillRectangle(backBrush, boundsf)

        If decValor <= m_intLimiteVerde Then

            g.DrawImage(imgListBarras.Images(0), CalculaGrafica(boundsreduc, decValor))

        ElseIf decValor <= m_intLimiteAmarillo Then

            g.DrawImage(imgListBarras.Images(1), CalculaGrafica(boundsreduc, decValor))

        Else

            g.DrawImage(imgListBarras.Images(2), CalculaGrafica(boundsreduc, decValor))

        End If

        If m_blnNegritaExtremos Then
            objFontExtremos = New Font(Me.DataGridTableStyle.DataGrid.Font, FontStyle.Bold)
        Else
            objFontExtremos = New Font(Me.DataGridTableStyle.DataGrid.Font, FontStyle.Regular)
        End If

        If m_blnNegritaValor Then
            objFontValor = New Font(Me.DataGridTableStyle.DataGrid.Font, FontStyle.Bold)
        Else
            objFontValor = New Font(Me.DataGridTableStyle.DataGrid.Font, FontStyle.Regular)
        End If

        If m_blnMostrarExtremos Then

            FormatTexto.LineAlignment = StringAlignment.Center
            FormatTexto.Alignment = StringAlignment.Near
            g.DrawString("0%", objFontExtremos, foreBrush, boundsf.X, (boundsf.Height / 2) + boundsf.Y, FormatTexto)

            FormatTexto.LineAlignment = StringAlignment.Center
            FormatTexto.Alignment = StringAlignment.Far
            g.DrawString("100%", objFontExtremos, foreBrush, boundsf.X + boundsf.Width, (boundsf.Height / 2) + boundsf.Y, FormatTexto)

        End If

        If m_blnMostrarValor Then

            FormatTexto.LineAlignment = StringAlignment.Center
            FormatTexto.Alignment = StringAlignment.Center
            g.DrawString(strValor, objFontValor, foreBrush, boundsf.X + (boundsf.Width / 2), (boundsf.Height / 2) + boundsf.Y, FormatTexto)

        End If

    End Sub

    Protected Overloads Overrides Sub Edit(ByVal source As System.Windows.Forms.CurrencyManager, ByVal rowNum As Integer, ByVal bounds As System.Drawing.Rectangle, ByVal [readOnly] As Boolean, ByVal instantText As String, ByVal cellIsVisible As Boolean)
        If m_blnAllowEdit Then
            MyBase.Edit(source, rowNum, bounds, [readOnly], instantText, cellIsVisible)
        End If
    End Sub

    Private Function CalculaGrafica(ByVal p_Bounds As Rectangle, ByVal p_decValor As Decimal) As Rectangle
        Dim BoundResult As Rectangle
        Dim intWidth As Integer
        Dim intWidthResult As Integer

        If p_decValor >= 100 Then
            BoundResult = p_Bounds
        Else
            intWidth = p_Bounds.Width

            intWidthResult = (p_decValor * intWidth) / 100

            If intWidthResult < 0 Then
                intWidthResult = 0
            End If

            BoundResult = New Rectangle(p_Bounds.X, p_Bounds.Y, intWidthResult, p_Bounds.Height)
        End If

        Return BoundResult
    End Function

#End Region

#Region "Propiedades"

    Public Property scgLimiteVerde() As Integer
        Get
            Return m_intLimiteVerde
        End Get
        Set(ByVal Value As Integer)
            m_intLimiteVerde = Value
        End Set
    End Property

    Public Property scgLimiteAmarillo() As Integer
        Get
            Return m_intLimiteAmarillo
        End Get
        Set(ByVal Value As Integer)
            m_intLimiteAmarillo = Value
        End Set
    End Property

    Public Property scgMostrarValor() As Boolean
        Get
            Return m_blnMostrarValor
        End Get
        Set(ByVal Value As Boolean)
            m_blnMostrarValor = Value
        End Set
    End Property

    Public Property scgMostrarExtremos() As Boolean
        Get
            Return m_blnMostrarExtremos
        End Get
        Set(ByVal Value As Boolean)
            m_blnMostrarExtremos = Value
        End Set
    End Property

    Public Property scgNegritaValor() As Boolean
        Get
            Return m_blnNegritaValor
        End Get
        Set(ByVal Value As Boolean)
            m_blnNegritaValor = Value
        End Set
    End Property

    Public Property scgNegritaExtremos() As Boolean
        Get
            Return m_blnNegritaExtremos
        End Get
        Set(ByVal Value As Boolean)
            m_blnNegritaExtremos = Value
        End Set
    End Property

    Public Property scgAllowEdit() As Boolean
        Get
            Return m_blnAllowEdit
        End Get
        Set(ByVal Value As Boolean)
            m_blnAllowEdit = Value
            If Not m_blnAllowEdit Then
                Me.ReadOnly = True
            End If
        End Set
    End Property

#End Region

End Class
