Imports System
Imports System.ComponentModel

Public Class DataGridColumnStylePadding

    Private m_left As Integer
    Private m_right As Integer
    Private m_top As Integer
    Private m_bottom As Integer


    Public Property Left() As Integer
        Get
            Return m_left
        End Get
        Set(ByVal Value As Integer)
            m_left = Value
        End Set
    End Property

    Public Property Right() As Integer
        Get
            Return m_right
        End Get
        Set(ByVal Value As Integer)
            m_right = Value
        End Set
    End Property

    Public Property Top() As Integer
        Get
            Return m_top
        End Get
        Set(ByVal Value As Integer)
            m_top = Value
        End Set
    End Property

    Public Property Bottom() As Integer
        Get
            Return m_bottom
        End Get
        Set(ByVal Value As Integer)
            m_bottom = Value
        End Set
    End Property

    Public Overloads Sub SetPadding(ByVal padValue As Integer)

        m_left = padValue
        m_right = padValue
        m_top = padValue
        m_bottom = padValue
    End Sub 'SetPadding


    Public Overloads Sub SetPadding(ByVal top As Integer, ByVal right As Integer, ByVal bottom As Integer, ByVal left As Integer)
        UpdatePaddingValues(top, right, bottom, left)
    End Sub 'SetPadding


    Public Sub New(ByVal padValue As Integer)
        Me.SetPadding(padValue)
    End Sub 'New


    Public Sub New(ByVal top As Integer, ByVal right As Integer, ByVal bottom As Integer, ByVal left As Integer)
        UpdatePaddingValues(top, right, bottom, left)
    End Sub 'New


    Private Sub UpdatePaddingValues(ByVal top As Integer, ByVal right As Integer, ByVal bottom As Integer, ByVal left As Integer)
        m_top = top
        m_right = right
        m_bottom = bottom
        m_left = left
    End Sub

End Class
