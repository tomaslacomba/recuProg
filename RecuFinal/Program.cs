public class Cliente
{
    private int numeroCliente;
    private string nombre;
    private List<Vehiculo> vehiculos;

    public Cliente(int numeroCliente, string nombre)
    {
        this.numeroCliente = numeroCliente;
        this.nombre = nombre;
        this.vehiculos = new List<Vehiculo>();
    }

    public int NumeroCliente
    {
        get { return numeroCliente; }
        private set { numeroCliente = value; }
    }

    public string Nombre
    {
        get { return nombre; }
        set { nombre = value; }
    }

    public IReadOnlyList<Vehiculo> Vehiculos
    {
        get { return vehiculos.AsReadOnly(); }
    }

    public void AgregarVehiculo(Vehiculo vehiculo)
    {
        vehiculos.Add(vehiculo);
    }

    public void MostrarVehiculosYTrabajos()
    {
        Console.WriteLine($"Cliente: {Nombre} - Número: {NumeroCliente}");
        foreach (var vehiculo in vehiculos)
        {
            Console.WriteLine($"\tVehículo: {vehiculo.Marca} {vehiculo.Modelo} ({vehiculo.Año})");
            foreach (var trabajo in vehiculo.Trabajos)
            {
                Console.WriteLine($"\t\tTrabajo: {trabajo.Descripcion} - Costo: ${trabajo.Costo}");
            }
        }
    }
}


public class Vehiculo
{
    private string marca;
    private string modelo;
    private int año;
    private List<TrabajoReparacion> trabajos;

    public Vehiculo(string marca, string modelo, int año)
    {
        this.marca = marca;
        this.modelo = modelo;
        this.año = año;
        this.trabajos = new List<TrabajoReparacion>();
    }

    public string Marca
    {
        get { return marca; }
        private set { marca = value; }
    }

    public string Modelo
    {
        get { return modelo; }
        private set { modelo = value; }
    }

    public int Año
    {
        get { return año; }
        private set { año = value; }
    }

    public IReadOnlyList<TrabajoReparacion> Trabajos
    {
        get { return trabajos.AsReadOnly(); }
    }

    public void AgregarTrabajo(TrabajoReparacion trabajo)
    {
        trabajos.Add(trabajo);
    }
}



public class TrabajoReparacion
{
    private string descripcion;
    private TipoReparacion tipo;
    private double costo;
    private Mecánico mecanicoAsignado;

    public TrabajoReparacion(string descripcion, TipoReparacion tipo, double costo, Mecánico mecanicoAsignado)
    {
        this.descripcion = descripcion;
        this.tipo = tipo;
        this.costo = costo;
        this.mecanicoAsignado = mecanicoAsignado;
    }

    public string Descripcion
    {
        get { return descripcion; }
        private set { descripcion = value; }
    }

    public TipoReparacion Tipo
    {
        get { return tipo; }
        private set { tipo = value; }
    }

    public double Costo
    {
        get { return costo; }
        private set { costo = value; }
    }

    public Mecánico MecanicoAsignado
    {
        get { return mecanicoAsignado; }
        private set { mecanicoAsignado = value; }
    }
}



public class Mecánico
{
    private string nombre;
    private TipoReparacion especialidad;

    public Mecánico(string nombre, TipoReparacion especialidad)
    {
        this.nombre = nombre;
        this.especialidad = especialidad;
    }

    public string Nombre
    {
        get { return nombre; }
        private set { nombre = value; }
    }

    public TipoReparacion Especialidad
    {
        get { return especialidad; }
        private set { especialidad = value; }
    }
}



public enum TipoReparacion
{
    Mecanica,
    Electrica,
    Pintura
}



public class SistemaTaller
{
    private List<Cliente> clientes;
    private List<Mecánico> mecanicos;

    public SistemaTaller()
    {
        this.clientes = new List<Cliente>();
        this.mecanicos = new List<Mecánico>();
    }

    public IReadOnlyList<Cliente> Clientes
    {
        get { return clientes.AsReadOnly(); }
    }

    public IReadOnlyList<Mecánico> Mecanicos
    {
        get { return mecanicos.AsReadOnly(); }
    }

    public void RegistrarCliente(Cliente cliente)
    {
        clientes.Add(cliente);
    }

    public void RegistrarMecanico(Mecánico mecanico)
    {
        mecanicos.Add(mecanico);
    }

    public Mecánico AsignarMecanico(TipoReparacion tipo)
    {
        foreach (var mecanico in mecanicos)
        {
            if (mecanico.Especialidad == tipo)
            {
                return mecanico;
            }
        }
        throw new Exception("No hay mecánicos disponibles para este tipo de reparación.");
    }

    public double CalcularTotalTrabajos(Cliente cliente)
    {
        double total = 0;
        foreach (var vehiculo in cliente.Vehiculos)
        {
            foreach (var trabajo in vehiculo.Trabajos)
            {
                total += trabajo.Costo;
            }
        }
        return total;
    }

    public void MostrarTrabajosCliente(Cliente cliente)
    {
        cliente.MostrarVehiculosYTrabajos();
        double total = CalcularTotalTrabajos(cliente);
        Console.WriteLine($"Total acumulado de trabajos: ${total}");
    }
}



public class Program
{
    public static void Main()
    {

        SistemaTaller taller = new SistemaTaller();


        taller.RegistrarMecanico(new Mecánico("Pedro", TipoReparacion.Mecanica));
        taller.RegistrarMecanico(new Mecánico("Carlos", TipoReparacion.Electrica));


        Cliente cliente1 = new Cliente(1, "Juan Pérez");
        Vehiculo vehiculo1 = new Vehiculo("Toyota", "Corolla", 2015);
        cliente1.AgregarVehiculo(vehiculo1);


        taller.RegistrarCliente(cliente1);


        Mecánico mecanicoMecanica = taller.AsignarMecanico(TipoReparacion.Mecanica);
        vehiculo1.AgregarTrabajo(new TrabajoReparacion("Cambio de aceite", TipoReparacion.Mecanica, 100, mecanicoMecanica));

        Mecánico mecanicoElectrico = taller.AsignarMecanico(TipoReparacion.Electrica);
        vehiculo1.AgregarTrabajo(new TrabajoReparacion("Reparación de luces", TipoReparacion.Electrica, 200, mecanicoElectrico));


        taller.MostrarTrabajosCliente(cliente1);
    }
}
