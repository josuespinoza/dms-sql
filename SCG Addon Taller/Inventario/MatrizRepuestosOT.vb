
'*******************************************
'*Matriz para manejo de los repuestos
'*******************************************

Imports SAPbouiCOM
Imports SCG.SBOFramework.UI

Public Class MatrizRepuestosOT
    : Inherits MatrixSBO

#Region "Declaraciones"

    'Columnas de la matriz
    Private _columnaCol_Cod As ColumnaMatrixSBOEditText(Of String)
    Private _columnaCol_Des As ColumnaMatrixSBOEditText(Of String)
    Private _columnaCol_Can As ColumnaMatrixSBOEditText(Of String)
    Private _columnaCol_Mon As ColumnaMatrixSBOEditText(Of String)
    Private _columnaCol_Pre As ColumnaMatrixSBOEditText(Of String)
    Private _columnaCol_Apr As ColumnaMatrixSBOEditText(Of String)
    Private _columnaCol_Tra As ColumnaMatrixSBOEditText(Of String)
    Private _columnaCol_Sel As ColumnaMatrixSBOCheckBox(Of String)
    Private _columnaCol_Per As ColumnaMatrixSBOCheckBox(Of String)
    Private _columnaCol_LN As ColumnaMatrixSBOEditText(Of String)
    Private _colCompra As ColumnaMatrixSBOEditText(Of String)
    Private _colCanRec As ColumnaMatrixSBOEditText(Of String)
    Private _colCanSol As ColumnaMatrixSBOEditText(Of String)
#End Region

#Region "Propiedades"
    'propiedades para las columnas de la matriz

    Public Property ColumnaColCod As ColumnaMatrixSBOEditText(Of String)
        Get
            Return _columnaCol_Cod
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of String))
            _columnaCol_Cod = value
        End Set
    End Property

    Public Property ColumnaColDes As ColumnaMatrixSBOEditText(Of String)
        Get
            Return _columnaCol_Des
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of String))
            _columnaCol_Des = value
        End Set
    End Property

    Public Property ColumnaColCan As ColumnaMatrixSBOEditText(Of String)
        Get
            Return _columnaCol_Can
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of String))
            _columnaCol_Can = value
        End Set
    End Property

    Public Property ColumnaColMon As ColumnaMatrixSBOEditText(Of String)
        Get
            Return _columnaCol_Mon
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of String))
            _columnaCol_Mon = value
        End Set
    End Property

    Public Property ColumnaColPre As ColumnaMatrixSBOEditText(Of String)
        Get
            Return _columnaCol_Pre
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of String))
            _columnaCol_Pre = value
        End Set
    End Property

    Public Property ColumnaColApr As ColumnaMatrixSBOEditText(Of String)
        Get
            Return _columnaCol_Apr
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of String))
            _columnaCol_Apr = value
        End Set
    End Property

    Public Property ColumnaColTra As ColumnaMatrixSBOEditText(Of String)
        Get
            Return _columnaCol_Tra
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of String))
            _columnaCol_Tra = value
        End Set
    End Property

    Public Property ColumnaColSel As ColumnaMatrixSBOCheckBox(Of String)
        Get
            Return _columnaCol_Sel
        End Get
        Set(ByVal value As ColumnaMatrixSBOCheckBox(Of String))
            _columnaCol_Sel = value
        End Set
    End Property

    Public Property ColumnaColPer As ColumnaMatrixSBOCheckBox(Of String)
        Get
            Return _columnaCol_Per
        End Get
        Set(ByVal value As ColumnaMatrixSBOCheckBox(Of String))
            _columnaCol_Per = value
        End Set
    End Property

    Public Property ColumnaColLN As ColumnaMatrixSBOEditText(Of String)
        Get
            Return _columnaCol_LN
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of String))
            _columnaCol_LN = value
        End Set
    End Property

    Public Property ColumnaCanRec() As ColumnaMatrixSBOEditText(Of String)
        Get
            Return _colCanRec
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of String))
            _colCanRec = value
        End Set
    End Property

    Public Property ColumnaCanSol() As ColumnaMatrixSBOEditText(Of String)
        Get
            Return _colCanSol
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of String))
            _colCanSol = value
        End Set
    End Property

    Public Property ColumnaColCom() As ColumnaMatrixSBOEditText(Of String)
        Get
            Return _colCompra
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of String))
            _colCompra = value
        End Set
    End Property

#End Region

#Region "Constructor"

    ''' <summary>
    ''' Constructor
    ''' </summary>
    ''' <param name="UniqueId">Nombre de la matriz</param>
    ''' <param name="formularioSBO">Objeto formulario</param>
    ''' <param name="tablaLigada">Tabla ligada</param>
    ''' <remarks></remarks>
    Public Sub New(ByVal UniqueId As String, ByVal formularioSBO As IForm, ByVal tablaLigada As String)
        MyBase.New(UniqueId, formularioSBO)
        Me.TablaLigada = tablaLigada
    End Sub

#End Region

#Region "Metodos"

    ''' <summary>
    ''' Crea las columnas para la matriz
    ''' </summary>
    ''' <remarks></remarks>
    Public Overrides Sub CreaColumnas()
        ColumnaColSel = New ColumnaMatrixSBOCheckBox(Of String)("Col_sel", True, "sel", Me)
        ColumnaColPer = New ColumnaMatrixSBOCheckBox(Of String)("Col_per", True, "per", Me)
        ColumnaColCod = New ColumnaMatrixSBOEditText(Of String)("Col_cod", True, "cod", Me)
        ColumnaColDes = New ColumnaMatrixSBOEditText(Of String)("Col_des", True, "des", Me)
        ColumnaColCan = New ColumnaMatrixSBOEditText(Of String)("Col_can", True, "can", Me)
        ColumnaColMon = New ColumnaMatrixSBOEditText(Of String)("Col_mon", True, "mon", Me)
        ColumnaColPre = New ColumnaMatrixSBOEditText(Of String)("Col_pre", True, "pre", Me)
        ColumnaColApr = New ColumnaMatrixSBOEditText(Of String)("Col_apr", True, "apr", Me)
        ColumnaColTra = New ColumnaMatrixSBOEditText(Of String)("Col_tra", True, "tra", Me)
        ColumnaColLN = New ColumnaMatrixSBOEditText(Of String)("Col_LN", True, "ln", Me)
        ColumnaColCom = New ColumnaMatrixSBOEditText(Of String)("Col_Com", True, "com", Me)
        ColumnaCanRec = New ColumnaMatrixSBOEditText(Of String)("Col_Rec", True, "rec", Me)
        ColumnaCanSol = New ColumnaMatrixSBOEditText(Of String)("Col_Sol", True, "sol", Me)
    End Sub

    ''' <summary>
    ''' Liga las columnas de la matriz
    ''' </summary>
    ''' <remarks></remarks>
    Public Overrides Sub LigaColumnas()
        ColumnaColSel.AsignaBindingDataTable()
        ColumnaColPer.AsignaBindingDataTable()
        ColumnaColCod.AsignaBindingDataTable()
        ColumnaColDes.AsignaBindingDataTable()
        ColumnaColCan.AsignaBindingDataTable()
        ColumnaColMon.AsignaBindingDataTable()
        ColumnaColPre.AsignaBindingDataTable()
        ColumnaColApr.AsignaBindingDataTable()
        ColumnaColTra.AsignaBindingDataTable()
        ColumnaColLN.AsignaBindingDataTable()
        ColumnaColCom.AsignaBindingDataTable()
        ColumnaCanRec.AsignaBindingDataTable()
        ColumnaCanSol.AsignaBindingDataTable()
    End Sub

#End Region

End Class
