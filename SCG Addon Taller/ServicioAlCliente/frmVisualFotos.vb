Imports Deklarit
Imports DMSOneFramework.SCGBusinessLogic
Imports DMSOneFramework.SCGDataAccess
Imports DMSOneFramework.SCGDataAccess.DAConexion
Imports DMSOneFramework
Imports DMSOneFramework.SCGCommon
Imports System.Data.SqlClient
Imports System.ComponentModel
Imports System.Drawing
Imports System.Windows.Forms
Imports System.Globalization
Imports System.Runtime.InteropServices
Imports System.Collections.Generic
Imports System.Text
Imports System.IO
'Imports System.Threading
'Imports System.Windows.Forms.PictureBox
Imports System.Data.DataTable
Imports System.Threading
'Imports SAPbouiCOM
'Imports Sunisoft.IrisSkin
'Imports SCG_User_Interface

Public Class frmVisualFotos

    Private m_dtFecha As String = String.Empty
    Public m_oCompany As SAPbobsCOM.Company
    Public m_oApplication As SAPbouiCOM.Application

    Private imgArray As System.Windows.Forms.PictureBox()
    Public Orden As String = String.Empty
    Public Shared ImageToShow As String
    Private NumOfFiles As Integer
    Private imgName As String()
    Private imgExtension As String()

    Private Dragging As Boolean
    Private xPos As Integer
    Private yPos As Integer

    Public Sub New()
        MyBase.New()
        InitializeComponent()
    End Sub

    Public Sub New(ByVal p_OT As String, ByVal p_oApplication As SAPbouiCOM.Application, ByVal p_oCompany As SAPbobsCOM.Company)

        MyBase.New()

        m_dtFecha = p_OT
        m_oApplication = p_oApplication
        m_oCompany = p_oCompany
        DMS_Connector.Helpers.SetCulture(Thread.CurrentThread.CurrentUICulture, My.Resources.Resource.Culture)
        
        InitializeComponent()
        Me.BringToFront()

    End Sub

    Private Function ThumbnailCallback() As Boolean
        Return False
    End Function

    Private Sub ClickImage(sender As System.Object, e As System.EventArgs)

        '    ImageToShow = ((System.Windows.Forms.PictureBox)sender).Tag.ToString();
        Dim ImageToShow As String = DirectCast(sender, PictureBox).Tag.ToString()

        'Ajuste de tamaño
        Dim bitmapFile As FileStream = New FileStream(ImageToShow.ToString(), FileMode.Open, FileAccess.Read, FileShare.ReadWrite)
        'FileStream bitmapFile = new FileStream(ImageToShow.ToString(), FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
        Dim loaded As Image = New Bitmap(bitmapFile)
        'Image(loaded = New Bitmap(bitmapFile))

        'Bitmap bmp = New Bitmap(240, 320))
        Dim bmp As Bitmap = New Bitmap(240, 320)
        'Graphics(g = Graphics.FromImage(loaded))
        Dim g As Graphics = g.FromImage(loaded)

        g.Clear(Color.Black)
        g.FillRectangle(New SolidBrush(Color.Black), 0, 100, 240, 103)

        PictureBox1.Width = 831
        PictureBox1.Height = 581
        PictureBox1.Top = 0
        PictureBox1.Left = 0
        PictureBox1.Image = Nothing
        PictureBox1.InitialImage = Nothing
        PictureBox1.SizeMode = PictureBoxSizeMode.StretchImage
        PictureBox1.Image = Image.FromFile(ImageToShow)

    End Sub

    Private Sub MuestraFotos(DireccionFoto As String, Posicion As Integer)
        Try
            Dim Xpos As Integer = 15
            Dim Ypos As Integer = 15

            Dim Foto As PictureBox = New System.Windows.Forms.PictureBox()
            Dim img As Image
            Dim myCallback As New Image.GetThumbnailImageAbort(AddressOf ThumbnailCallback)


            Dim Ext As String() = New String() {".GIF", ".JPG", ".BMP", ".PNG"}

            'Foto = New PictureBox

            img = Image.FromFile(DireccionFoto)
            Foto.Image = img.GetThumbnailImage(120, 120, myCallback, IntPtr.Zero)
            img = Nothing
            If Posicion > 1 Then
                Ypos = Ypos + (130 * (Posicion - 1))
            End If

            Foto.Left = Xpos
            Foto.Top = Ypos
            Foto.Width = 120
            Foto.Height = 120
            Foto.Visible = True

            Foto.Tag = DireccionFoto
            AddHandler Foto.Click, AddressOf ClickImage
            Me.BackPanel.Controls.Add(Foto)
            Xpos = Xpos + 72

        Catch EX As Exception

        End Try

    End Sub

    Private Sub frmVisualFotos_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
        Dim imagenes As System.Data.DataTable
        Dim m_strConsulta As String = "SELECT U_Direccion as Foto FROM [@SCGD_OT] T0 , [@SCGD_IMG_OT] T1 Where T0.Code = T1.Code and T0.Code = '" + m_dtFecha.ToString() + "' Order by T1.LineId "

        ' lblNombreAgenda.Text = "Visualizacion de Orden de trabajo : " + m_dtFecha

        imagenes = Utilitarios.EjecutarConsultaDataTable(m_strConsulta, m_oCompany.CompanyDB, m_oCompany.Server)
        'DA_Conexion.EjecutarConsultaDataTable(m_strConsulta, true);

        If (imagenes.Rows.Count > 0) Then
            ' for (int i = 0; i <= imagenes.Rows.Count -1; i++)
            For i As Integer = 0 To imagenes.Rows.Count

                Dim direccion As String = imagenes.Rows(i)("Foto").ToString()
                MuestraFotos(direccion, i + 1)
            Next
            btnAcerca.Enabled = True
            btnAleja.Enabled = True
            bntCopia.Enabled = True
            PictureBox1.Enabled = True
        Else
            btnAcerca.Enabled = False
            btnAleja.Enabled = False
            bntCopia.Enabled = False
            PictureBox1.Enabled = False
        End If
        Me.TopMost = True
    End Sub

    Private Sub btnAcerca_Click(sender As System.Object, e As System.EventArgs) Handles btnAcerca.Click
        Dim zoomRatio As Integer = 10
        Dim widthZoom As Integer = (PictureBox1.Width * zoomRatio / 100)
        Dim heightZoom As Integer = (PictureBox1.Height * zoomRatio / 100)

        widthZoom *= 1
        heightZoom *= 1

        PictureBox1.Width += widthZoom
        PictureBox1.Height += heightZoom
    End Sub

    Private Sub btnAleja_Click(sender As System.Object, e As System.EventArgs) Handles btnAleja.Click
        Dim zoomRatio As Integer = 10
        Dim widthZoom As Integer = (PictureBox1.Width * zoomRatio / 100)
        Dim heightZoom As Integer = (PictureBox1.Height * zoomRatio / 100)
        widthZoom *= -1
        heightZoom *= -1
        PictureBox1.Width += widthZoom
        PictureBox1.Height += heightZoom
    End Sub

    Private Sub bntCopia_Click(sender As System.Object, e As System.EventArgs) Handles bntCopia.Click

        Try
            Clipboard.SetDataObject(PictureBox1.Image)

        Catch ex As Exception
            'Mensajes.Mensajes.ManejaMensajes(Resource.noimagen, 5, Mensajes.Mensajes.TipoMensaje.Informacion)
        End Try

    End Sub

    Private Sub PictureBox1_MouseDown(sender As System.Object, e As System.Windows.Forms.MouseEventArgs) Handles PictureBox1.MouseDown
        If (e.Button = MouseButtons.Left) Then
            Dragging = True
            xPos = e.X
            yPos = e.Y
        End If
    End Sub

    Private Sub PictureBox1_MouseClick(sender As System.Object, e As System.Windows.Forms.MouseEventArgs) Handles PictureBox1.MouseClick
        Dim c As Control = DirectCast(sender, Control)
        'Control c = sender as Control;
        If (Dragging AndAlso c IsNot Nothing) Then
            c.Top = e.Y + c.Top - yPos
            c.Left = e.X + c.Left - xPos
        End If
    End Sub

    Private Sub PictureBox1_MouseMove(sender As System.Object, e As System.Windows.Forms.MouseEventArgs) Handles PictureBox1.MouseMove
        Try
            If e.Button = MouseButtons.Left Then
                Dim currentMousePos As Point = e.Location
                Dim distanceX As Integer = currentMousePos.X - e.X
                Dim distanceY As Integer = currentMousePos.Y - e.Y
                Dim newX As Integer = PictureBox1.Location.X + distanceX
                Dim newY As Integer = PictureBox1.Location.Y + distanceY

                If newX + PictureBox1.Image.Width < PictureBox1.Image.Width AndAlso PictureBox1.Image.Width + newX > PictureBox1.Width Then
                    PictureBox1.Location = New Point(newX, PictureBox1.Location.Y)
                End If
                If newY + PictureBox1.Image.Height < PictureBox1.Image.Height AndAlso PictureBox1.Image.Height + newY > PictureBox1.Height Then
                    PictureBox1.Location = New Point(PictureBox1.Location.X, newY)
                End If
            End If

        Catch EX As Exception
        End Try
    End Sub

    Private Sub BackPanel_Paint(sender As System.Object, e As System.Windows.Forms.PaintEventArgs) Handles BackPanel.Paint

    End Sub
End Class