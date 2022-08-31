Imports SAPbouiCOM
Imports SCG.SBOFramework.UI

Public Class MatrizHistCV : Inherits MatrixSBO

#Region "Declaraciones"


    Private _columnaCol_Usr As ColumnaMatrixSBOEditText(Of String)
    Private _columnaCol_Niv As ColumnaMatrixSBOEditText(Of String)
    Private _columnaCol_Fec As ColumnaMatrixSBOEditText(Of String)
    Private _columnaCol_Hor As ColumnaMatrixSBOEditText(Of String)
    Private _columnaCol_Com As ColumnaMatrixSBOEditText(Of String)

#End Region

#Region "Propiedades"

    

    Public Property ColumnaColUsr As ColumnaMatrixSBOEditText(Of String)
        Get
            Return _columnaCol_Usr
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of String))
            _columnaCol_Usr = value
        End Set
    End Property

    Public Property ColumnaColNivel As ColumnaMatrixSBOEditText(Of String)
        Get
            Return _columnaCol_Niv
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of String))
            _columnaCol_Niv = value
        End Set
    End Property

    Public Property ColumnaColFecha As ColumnaMatrixSBOEditText(Of String)
        Get
            Return _columnaCol_Fec
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of String))
            _columnaCol_Fec = value
        End Set
    End Property

    Public Property ColumnaColHora As ColumnaMatrixSBOEditText(Of String)
        Get
            Return _columnaCol_Hor
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of String))
            _columnaCol_Hor = value
        End Set
    End Property

    Public Property ColumnaColComentarios As ColumnaMatrixSBOEditText(Of String)
        Get
            Return _columnaCol_Com
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of String))
            _columnaCol_Com = value
        End Set
    End Property

#End Region

#Region "Constructor"

    Public Sub New(ByVal UniqueId As String, ByVal formularioSBO As IForm, ByVal tablaLigada As String)
        MyBase.New(UniqueId, formularioSBO)
        Me.TablaLigada = tablaLigada
    End Sub

#End Region

#Region "Métodos"

    Public Overrides Sub CreaColumnas()

        ColumnaColUsr = New ColumnaMatrixSBOEditText(Of String)("Col_Usr", True, "Col_Usr", Me)
        ColumnaColNivel = New ColumnaMatrixSBOEditText(Of String)("Col_Niv", True, "Col_Niv", Me)
        ColumnaColFecha = New ColumnaMatrixSBOEditText(Of String)("Col_Dat", True, "Col_Dat", Me)
        ColumnaColHora = New ColumnaMatrixSBOEditText(Of String)("Col_Hor", True, "Col_Hor", Me)
        ColumnaColComentarios = New ColumnaMatrixSBOEditText(Of String)("Col_Com", True, "Col_Com", Me)

    End Sub

    Public Overrides Sub LigaColumnas()

        ColumnaColUsr.AsignaBindingDataTable()
        ColumnaColNivel.AsignaBindingDataTable()
        ColumnaColFecha.AsignaBindingDataTable()
        ColumnaColHora.AsignaBindingDataTable()
        ColumnaColComentarios.AsignaBindingDataTable()
    End Sub

#End Region
End Class
