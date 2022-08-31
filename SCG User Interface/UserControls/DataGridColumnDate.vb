Public Class DataGridColumnDate
    Inherits DataGridTextBoxColumn

#Region "Declaraciones"

    Private m_DTPicker As DateTimePicker
    Private m_intCellAnterior As Integer

    ''Variables de Propiedades
    Private m_CalendarForeColor As Color
    Private m_CalendarMonthBackground As Color
    Private m_CalendarTitleBackColor As Color
    Private m_CalendarTitleForeColor As Color
    Private m_CalendarTrailingForeColor As Color
    Private m_ShowUpDown As Boolean

    Public Event CambiaValor(ByVal p_intRowNum As Integer)

#End Region

#Region "Constructor"

    Public Sub New()

        m_DTPicker = New DateTimePicker

        With m_DTPicker
            .Visible = False
            .Format = DateTimePickerFormat.Custom
            .CustomFormat = Me.Format
            .Value = Now.Date
        End With

    End Sub

#End Region

#Region "Propiedades"

    Public Property CalendarForeColor() As Color
        Get
            Return m_CalendarForeColor
        End Get
        Set(ByVal Value As Color)
            m_CalendarForeColor = Value
        End Set
    End Property

    Public Property CalendarMonthBackground() As Color
        Get
            Return m_CalendarMonthBackground
        End Get
        Set(ByVal Value As Color)
            m_CalendarMonthBackground = Value
        End Set
    End Property

    Public Property CalendarTitleBackColor() As Color
        Get
            Return m_CalendarTitleBackColor
        End Get
        Set(ByVal Value As Color)
            m_CalendarTitleBackColor = Value
        End Set
    End Property

    Public Property CalendarTitleForeColor() As Color
        Get
            Return m_CalendarTitleForeColor
        End Get
        Set(ByVal Value As Color)
            m_CalendarTitleForeColor = Value
        End Set
    End Property

    Public Property CalendarTrailingForeColor() As Color
        Get
            Return m_CalendarTrailingForeColor
        End Get
        Set(ByVal Value As Color)
            m_CalendarTrailingForeColor = Value
        End Set
    End Property

    Public Property ShowUpDown() As Boolean
        Get
            Return m_ShowUpDown
        End Get
        Set(ByVal Value As Boolean)
            m_ShowUpDown = Value
        End Set
    End Property

#End Region

#Region "Procedimientos"

    Protected Overrides Sub Abort(ByVal rowNum As Integer)
        m_DTPicker.Visible = False
    End Sub

    Protected Overrides Function Commit(ByVal dataSource As System.Windows.Forms.CurrencyManager, ByVal rowNum As Integer) As Boolean
        Dim blnCambia As Boolean = False

        If m_intCellAnterior = rowNum Then

            If Not Me.GetColumnValueAtRow(dataSource, rowNum) Is DBNull.Value Then

                If Me.GetColumnValueAtRow(dataSource, rowNum) <> m_DTPicker.Value Then

                    Me.SetColumnValueAtRow(dataSource, rowNum, m_DTPicker.Value)

                    blnCambia = True

                End If
            Else

                Me.SetColumnValueAtRow(dataSource, rowNum, m_DTPicker.Value)

                blnCambia = True

            End If

        End If

        m_DTPicker.Visible = False

        If blnCambia Then
            RaiseEvent CambiaValor(rowNum)
        End If

        Return True

    End Function

    Protected Overloads Overrides Sub Edit(ByVal [source] As CurrencyManager, ByVal rowNum As Integer, ByVal bounds As Rectangle, ByVal [readOnly] As Boolean, ByVal instantText As String, ByVal cellIsVisible As Boolean)

        If cellIsVisible Then

            If Not Me.ReadOnly Then

                m_DTPicker.Font = Me.DataGridTableStyle.DataGrid.Font
                m_DTPicker.CalendarFont = m_DTPicker.Font
                m_DTPicker.CalendarForeColor = IIf(m_CalendarForeColor.IsEmpty, m_DTPicker.CalendarForeColor, m_CalendarForeColor)
                m_DTPicker.CalendarMonthBackground = IIf(m_CalendarMonthBackground.IsEmpty, m_DTPicker.CalendarMonthBackground, m_CalendarMonthBackground)
                m_DTPicker.CalendarTitleBackColor = IIf(m_CalendarTitleBackColor.IsEmpty, m_DTPicker.CalendarTitleBackColor, m_CalendarTitleBackColor)
                m_DTPicker.CalendarTitleForeColor = IIf(m_CalendarTitleForeColor.IsEmpty, m_DTPicker.CalendarTitleForeColor, m_CalendarTitleForeColor)
                m_DTPicker.CalendarTrailingForeColor = IIf(m_CalendarTrailingForeColor.IsEmpty, m_DTPicker.CalendarTrailingForeColor, m_CalendarTrailingForeColor)
                m_DTPicker.ShowUpDown = m_ShowUpDown

                m_DTPicker.Location = New Point(bounds.X + 1, bounds.Y + 1)
                m_DTPicker.Width = bounds.Width

                CargarEstiloPicker(m_DTPicker)

                m_DTPicker.Visible = True

                If Me.GetColumnValueAtRow([source], rowNum) Is DBNull.Value Then
                    m_DTPicker.Value = Now
                Else
                    m_DTPicker.Value = Me.GetColumnValueAtRow([source], rowNum)
                End If

                m_intCellAnterior = rowNum

            End If
        Else

            m_DTPicker.Visible = False

        End If
    End Sub

    Protected Overloads Overrides Sub Paint(ByVal g As System.Drawing.Graphics, ByVal bounds As System.Drawing.Rectangle, ByVal [source] As CurrencyManager, ByVal rowNum As Integer, ByVal backBrush As System.Drawing.Brush, ByVal foreBrush As System.Drawing.Brush, ByVal alignToRight As Boolean)
        Dim sf As New StringFormat
        Dim strTexto As String
        Dim objFont As Font

        sf.Alignment = StringAlignment.Near
        sf.LineAlignment = StringAlignment.Near

        strTexto = String.Format("{0:" & Me.Format & "}", Me.GetColumnValueAtRow([source], rowNum))

        objFont = Me.DataGridTableStyle.DataGrid.Font

        g.FillRectangle(backBrush, bounds)

        g.DrawString(strTexto, objFont, foreBrush, bounds.X, bounds.Y, sf)

    End Sub

    Protected Overrides Sub SetDataGridInColumn(ByVal value As DataGrid)

        MyBase.SetDataGridInColumn(value)

        If Not value.Controls.Contains(m_DTPicker) Then
            value.Controls.Add(m_DTPicker)
        End If
    End Sub

    Private Sub CargarEstiloPicker(ByRef p_DTPicker As DateTimePicker)

        With p_DTPicker
            .CustomFormat = Me.Format
        End With

    End Sub

#End Region

#Region "Eventos"

#End Region

End Class
