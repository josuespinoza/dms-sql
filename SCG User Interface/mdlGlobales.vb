Module mdlGlobales


    Public Sub cargarcomboServicioGrúa(ByVal cbo As ComboBox)
        cbo.Items.Clear()
        cbo.Items.Add("Servicio de ingreso al taller")
        cbo.Items.Add("Servicio para valoración")
        cbo.Items.Add("Servicio para revisión mecánica")
        cbo.Items.Add("Servicio para VB")
        cbo.Items.Add("Otros Servicios")

    End Sub

    Public Sub cargarcomboPiezaPrincipal(ByVal cbo As ComboBox)
        cbo.Items.Clear()

        cbo.Items.Add("Faroles")
        cbo.Items.Add("Guardabarro")
        cbo.Items.Add("Forro interior metálico")
        cbo.Items.Add("Paral")
        cbo.Items.Add("Puerta")
        cbo.Items.Add("Suspensión")
        cbo.Items.Add("Dirección")
        cbo.Items.Add("Aro de Rueda")
        cbo.Items.Add("Eje de Tracción")

    End Sub

    Public Sub cargarComboCobertura(ByVal cbo As ComboBox)
        cbo.Items.Clear()

        cbo.Items.Add("C: Daños a terceros")
        cbo.Items.Add("D: Directo")
        cbo.Items.Add("F: Robo")
        cbo.Items.Add("H: Vandalismo")
        cbo.Items.Add("D: Directo")

    End Sub

    'Public Sub cargarComboEstadoVisita(ByVal cboEstado As ComboBox)
    '    cboEstado.Items.Clear()

    '    cboEstado.Items.Add("Estudio aseguradora")
    '    cboEstado.Items.Add("Estudio cliente")
    '    cboEstado.Items.Add("Proceso")
    '    cboEstado.Items.Add("Trámite de cobro")
    '    cboEstado.Items.Add("Terminado")
    '    cboEstado.Items.Add("Repuestos pendientes")
    '    cboEstado.Items.Add("Cerrado")

    'End Sub

    Public Sub cargarComboEstadoOrden(ByVal cboEstado As ComboBox, ByVal Todos As Boolean)
        'Limpiar el data source, no el control como tal, ya que este
        'no posee el control de la fuente de datos.
        cboEstado.DataSource = Nothing
        SCG_User_Interface.clsUtilidadCombos.CargarComboEstadoOrdenes(cboEstado, Todos)


    End Sub

    Public Sub cargarComboPrioridadOrden(ByVal cboPrioridad As ComboBox)
        cboPrioridad.Items.Clear()

        cboPrioridad.Items.Add("Alta")
        cboPrioridad.Items.Add("Media")
        cboPrioridad.Items.Add("Baja")
        cboPrioridad.Items.Add("")

    End Sub

    Public Sub cargarComboRazonSusp(ByVal cboSuspension As ComboBox)
        cboSuspension.Items.Clear()

        cboSuspension.Items.Add("Falta colaboradores")
        cboSuspension.Items.Add("Falta de suministros")
        cboSuspension.Items.Add("Ingreso de órdenes más urgentes")
        cboSuspension.Items.Add("Falta de espacio")
        cboSuspension.Items.Add("Reproceso interno")

    End Sub

    Public Sub cargarComboEstadoRepuestos(ByVal cbo As ComboBox)
        cbo.Items.Clear()

        cbo.Items.Add("Pedido agencia")
        cbo.Items.Add("Pedido importador")
        cbo.Items.Add("Recibido")
        cbo.Items.Add("Devuelto")
        cbo.Items.Add("Excluido")

    End Sub

    Public Sub cargarComboRazonRep(ByVal cboFases As ComboBox)
        cboFases.Items.Clear()

        cboFases.Items.Add("Golpe mal enderezado")
        cboFases.Items.Add("Repuesto mal colocado")
        cboFases.Items.Add("Luces en mal estado")
        cboFases.Items.Add("Pintura inadecuada")

    End Sub

    Public Sub cargarComboActividades(ByVal cboActividad As ComboBox)
        cboActividad.Items.Clear()

        cboActividad.Items.Add("Descolocar y colocar")
        cboActividad.Items.Add("Enderezar y lijar")
        cboActividad.Items.Add("Sellar y remachar")
        cboActividad.Items.Add("Pintar")
        cboActividad.Items.Add("Pulir")
        cboActividad.Items.Add("Revisar Ddetalle")

    End Sub

    Public Sub cargarComboFases(ByVal cboFases As ComboBox)
        cboFases.Items.Clear()

        cboFases.Items.Add("Desarme, armado y mecánica")
        cboFases.Items.Add("Enderezado")
        cboFases.Items.Add("Pintura")
        cboFases.Items.Add("Detalle y control de calidad")

    End Sub

    Public Sub cargarComboCentroCosto(ByVal cboCentroCosto As ComboBox)
        cboCentroCosto.Items.Clear()

        cboCentroCosto.Items.Add("Valoración")
        cboCentroCosto.Items.Add("Desarme")
        cboCentroCosto.Items.Add("Enderezado")
        cboCentroCosto.Items.Add("Pintura")
        cboCentroCosto.Items.Add("Calidad")
        cboCentroCosto.Items.Add("Proveduría")
        cboCentroCosto.Items.Add("Bodegas")

    End Sub

    Public Sub cargarComnboSecciones(ByVal cboSecciones As ComboBox)
        cboSecciones.Items.Clear()

        cboSecciones.Items.Add("Nuevo Item")
        cboSecciones.Items.Add("Delantera izquierda")
        cboSecciones.Items.Add("Delantera central")
        cboSecciones.Items.Add("Delantera derecha")
        cboSecciones.Items.Add("Central izquierda")
        cboSecciones.Items.Add("Central central")
        cboSecciones.Items.Add("Central derecha")
        cboSecciones.Items.Add("Trasera izquierda")
        cboSecciones.Items.Add("Trasera central")
        cboSecciones.Items.Add("Trasera derecha")

    End Sub

    Public Sub cargarComnboTipoOrden(ByVal cboTipo As ComboBox)
        cboTipo.Items.Clear()

        cboTipo.Items.Add("Adicional")
        cboTipo.Items.Add("Asegurada")
        cboTipo.Items.Add("Personal")
        cboTipo.Items.Add("Repuesto pendiente")
        cboTipo.Items.Add("Reproceso externo")

    End Sub

    Public Sub cargarComboDias(ByVal cboDia As ComboBox)
        cboDia.Items.Clear()

        cboDia.Items.Add(1)
        cboDia.Items.Add(2)
        cboDia.Items.Add(3)
        cboDia.Items.Add(4)
        cboDia.Items.Add(5)
        cboDia.Items.Add(6)
        cboDia.Items.Add(7)
        cboDia.Items.Add(8)
        cboDia.Items.Add(9)
        cboDia.Items.Add(10)
        cboDia.Items.Add(11)
        cboDia.Items.Add(12)
        cboDia.Items.Add(13)
        cboDia.Items.Add(14)
        cboDia.Items.Add(15)
        cboDia.Items.Add(16)
        cboDia.Items.Add(17)
        cboDia.Items.Add(18)
        cboDia.Items.Add(19)
        cboDia.Items.Add(20)
        cboDia.Items.Add(21)
        cboDia.Items.Add(23)
        cboDia.Items.Add(24)
        cboDia.Items.Add(25)
        cboDia.Items.Add(26)
        cboDia.Items.Add(27)
        cboDia.Items.Add(28)
        cboDia.Items.Add(29)
        cboDia.Items.Add(30)
        cboDia.Items.Add(31)
    End Sub

    Public Sub cargarComboMeses(ByVal cboMes As ComboBox)
        cboMes.Items.Clear()

        cboMes.Items.Add("Enero")
        cboMes.Items.Add("Febrero")
        cboMes.Items.Add("Marzo")
        cboMes.Items.Add("Abril")
        cboMes.Items.Add("Mayo")
        cboMes.Items.Add("Junio")
        cboMes.Items.Add("Julio")
        cboMes.Items.Add("Agosto")
        cboMes.Items.Add("Septiembre")
        cboMes.Items.Add("Octubre")
        cboMes.Items.Add("Noviembre")
        cboMes.Items.Add("Diciembre")
    End Sub

    Public Sub cargarComboAnos(ByVal cboAno As ComboBox)
        cboAno.Items.Clear()

        cboAno.Items.Add("2004")
        cboAno.Items.Add("2005")
        cboAno.Items.Add("2006")
        cboAno.Items.Add("2007")
        cboAno.Items.Add("2008")
        cboAno.Items.Add("2009")
        cboAno.Items.Add("2010")
    End Sub

    Public Sub cargarComboAnosModelos(ByVal cboAno As ComboBox)
        cboAno.Items.Clear()

        cboAno.Items.Add("1975")
        cboAno.Items.Add("1976")
        cboAno.Items.Add("1977")
        cboAno.Items.Add("1978")
        cboAno.Items.Add("1980")
        cboAno.Items.Add("1981")
        cboAno.Items.Add("1982")
        cboAno.Items.Add("1983")
        cboAno.Items.Add("1984")
        cboAno.Items.Add("1985")
        cboAno.Items.Add("1986")
        cboAno.Items.Add("1987")
        cboAno.Items.Add("1988")
        cboAno.Items.Add("1989")
        cboAno.Items.Add("1990")
        cboAno.Items.Add("1991")
        cboAno.Items.Add("1992")
        cboAno.Items.Add("1993")
        cboAno.Items.Add("1994")
        cboAno.Items.Add("1995")
        cboAno.Items.Add("1996")
        cboAno.Items.Add("1997")
        cboAno.Items.Add("1998")
        cboAno.Items.Add("1999")
        cboAno.Items.Add("2000")
        cboAno.Items.Add("2001")
        cboAno.Items.Add("2002")
        cboAno.Items.Add("2003")
        cboAno.Items.Add("2004")
        cboAno.Items.Add("2005")
        cboAno.Items.Add("2006")
        cboAno.Items.Add("2007")
        cboAno.Items.Add("2008")
        cboAno.Items.Add("2009")
        cboAno.Items.Add("2010")

    End Sub

    Public Sub cargarComboMarcas(ByVal cboMarca As ComboBox)
        cboMarca.Items.Clear()

        cboMarca.Items.Add("BMW")
        cboMarca.Items.Add("Toyota")
        cboMarca.Items.Add("Nissan")
        cboMarca.Items.Add("Subarú")
        cboMarca.Items.Add("Renault")
        cboMarca.Items.Add("Peuggeot")
        cboMarca.Items.Add("Range Rover")

    End Sub

    Public Sub cargarComboModelos(ByVal cboModelo As ComboBox)
        cboModelo.Items.Clear()

        cboModelo.Items.Add("Legacy")
        cboModelo.Items.Add("Impreza WRX")
        cboModelo.Items.Add("Impreza GT")
        cboModelo.Items.Add("Impreza STI")

    End Sub


    Public Sub CargarComboEstado(ByVal cboEstado As ComboBox)
        cboEstado.Items.Clear()

        cboEstado.Items.Add("No Iniciada")
        cboEstado.Items.Add("Proceso")
        cboEstado.Items.Add("Suspendida")
        cboEstado.Items.Add("Finalizada")
    End Sub




End Module


