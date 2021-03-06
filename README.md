**_INTELIGENCIA ARTIFICIAL PARA VIDEOJUEGOS_** <br>
Integrado por:
- Tatiana Duarte Balv铆s
- Adri谩n de Lucas G贸mez
<br><br>

# __FightIA__
## **_IA para practicar en juegos de pelea_ 馃馃徎馃挜馃憚馃Ψ**

<br>

## __Introducci贸n__

FightIA es un proyecto planteado como un juego con una Inteligencia Artificial simple para iniciarse en los juegos de pelea y poder mejorar tus estrategias como jugador. Para ello, hemos realizado una labor de investigaci贸n sobre c贸mo se desarrollan las IA en este tipo de juegos y nos hemos basado en grandes cl谩sicos y referentes del g茅nero, como Street Fighter II, que a pesar de no contar con una IA demasiado compleja como es el caso de los juegos actuales, alcanz贸 gran fama mundial debido a otros factores como los moveset y combos.

<br>
<hr>

## __驴C贸mo suelen hacerse las IAs para este tipo de juegos?__

Para hacer la inteligencia artificial de este g茅nero de videojuegos se suele optar por un enfoque que hace que la IA 鈥渧ea鈥? lo que ocurre en pantalla como lo har铆a un humano. Podr铆a ver la posici贸n de los personajes, la vida que tiene cada uno, el tiempo que queda y ver qu茅 acci贸n est谩 realizando el contrincante.
A esta 鈥渧isi贸n鈥? se la denomina como **espacio de observaci贸n** y hay dos formas de implementarlo:

- **La cl谩sica**: poner toda esa informaci贸n en coordenadas y datos que la IA pueda entender.
- **La moderna**: realizando una captura tras cada acci贸n y dejar que sea la propia IA la que analice lo que ocurre en esa imagen y determine c贸mo debe responder a ello.

Nuestra IA est谩 basada en el modelo cl谩sico ya que quer铆amos que fuera m谩s similar a lo que se podr铆a haber hecho en la 茅poca del Street Fighter 2. Cabe destacar que el modelo moderno es considerado mejor ya que trabaja de una forma m谩s similar a como lo har铆a una persona siendo obviamente m谩s realista pero por contraparte esto lleva mucho m谩s tiempo de entrenamiento para perfeccionarlo. <br>
En conjunci贸n a esta t茅cnica se suele usar el **Machine Learning** para que tras cientos o miles de partidas la IA identifique patrones de c贸mo juegan los jugadores para as铆 saber reaccionar mejor a las diferentes estrategias que se le planteen.

<br>
<hr>

## __Street Fighter II como referente__

Hoy en d铆a dir铆amos que la Inteligencia Artificial en **Street Fighter II** no es nada muy sofisticado, ya que normalmente cuando se habla de IA mucha gente piensa en el **Machine Learning** mencionado anteriormente. Sin embargo, no existe nada como eso en SF2 y otros juegos de la 茅poca.

<br>
<img src="./Media/StreetFighterIA.PNG">
<br> <br>

Los personajes de SF2 no realizan movimientos y acciones de forma independiente, sino que estos se agrupan en peque帽os scripts escritos en 鈥渓enguaje m谩quina鈥? englob谩ndose as铆 en rutinas. Por ejemplo, una rutina t铆pica de ataques de un personaje ser谩 una lista de acciones como atacar, esperar, moverse y otras acciones dependientes del estado del jugador.

Esta IA act煤a de tres formas distintas: <br>
- <i>Esperar a ser atacada</i>: Se eligen scripts de forma aleatoria, priorizando movimiento hacia delante y detr谩s peque帽as distancias.
- <i>Atacar</i>: Se elige un script de rutina de ataque como el de la captura anterior.
- <i>Reaccionar a un ataque</i>: Se eligen scripts adecuados para evitar y esquivar el ataque dependiendo tambi茅n de la dificultad para dar margen de error.

<br>
<hr>

## __Estrategias comunes en una partida__

Cada jugador tiene su estrategia y su forma propia de jugar en los juegos de pelea. Estas estrategias se pueden agrupar en cuatro categor铆as que podemos usar de ejemplo a la hora de programar una IA para diferenciarla de otras y darle un aspecto m谩s humano a la hora de elegir movimientos y ataques:

- **Rushdowns**: Ataques constantes a corta distancia. Es una forma de jugar agresiva para confundir al contrincante y que no tenga mucho tiempo para pensar, lo que lo har谩 fallar m谩s a menudo y favorecer al jugador.
- **Footsies**: Mucho movimiento de adelante a atr谩s midiendo distancias con el objetivo de encontrar un hueco o momento oportuno para atacar.
- **Zoning**: Ataques a larga distancia para mantenerse lejos del contrincante, evitando as铆 muchas posibilidades de ser atacado.
- **Setups**: Preparar el terreno para encontrar el mejor momento para realizar una serie de ataques o combos.

<br>
<hr>

## __Desarrollo del proyecto__

Para el desarrollo de este proyecto de investigaci贸n sobre la IA en los juegos de pelea hemos desarrollado un prototipo en Unity en el cual se ha implementado una inteligencia artificial que controla al esqueleto mientras que el jugador controla a la chica. Se ha implementado todo a base de scripts ya que el objetivo era hacer una IA al estilo m谩s cl谩sico, por lo que el uso de Bolt y Behaviour Designer no estaba en nuestros planes.

Para nuestra IA quer铆amos que siguiera el modelo cl谩sico por an谩lisis de datos codificados, pero que tuviera implementadas ciertas estrategias que la hagan parecer m谩s humana y m谩s estrat茅gica. Las estrategias que decidimos integrar son **Footsies** y **Zoning** ya que creemos que son comportamientos muy t铆picos de una persona que juega de una forma m谩s defensiva. Ese tipo de estrategia sumado a la toma de decisiones de la inteligencia artificial que tienden, si la situaci贸n lo permite, a ser ofensiva, da como resultado una IA relativamente equilibrada, similar a lo que ser铆a un jugador promedio siendo apta para distintos tipos de jugadores.

<br>

## __Funcionamiento de FightIA__
Como base hemos separado el movimiento del personaje y las acciones que podemos realizar para asemejarse a como lo har铆a una persona con un mando, es decir, usando una mano para mover al personaje y la otra para dar los comandos de acci贸n. Para ello tenemos dos m茅todos, SigienteMovimiento y SiguienteAcci贸n que se encargan de procesar el estado actual y actuar en consecuencia de la mejor forma posible. Ambos son llamados por temporizadores independientes para simular el tiempo que una persona tarda en pensar su siguiente movimiento.

- **SiguienteMovimiento**: Mantiene la IA a una distancia prudente del jugador. Teniendo en cuenta la distancia que los separa, intenta no estar muy cerca constantemente para evitar ser atacada y tambi茅n no alejarse demasiado para poder actuar m谩s r谩pido. Si est谩 a una distancia 鈥渘ormal鈥? del jugador, se mantiene en movimiento (estrategia **Footsies**). Las distancias m铆nima y m谩xima que considera como prudentes se pueden modificar desde el editor de Unity para realizar pruebas.
Los movimientos posibles entre los que decidir no son muchos al tratarse de un juego en 2D: acercarse, alejarse o mantenerse quieto.

- **SiguienteAcci贸n**: Seg煤n el estado en el que se encuentra el enemigo y teniendo en cuenta par谩metros como la posici贸n de los personajes, la distancia y la vida de cada uno se calcula cual es la orden m谩s 贸ptima a dar en cada situaci贸n, pero para meter un poco de variaci贸n y error 鈥渉umano鈥? estas 贸rdenes estar谩n influenciadas por una probabilidad y porcentajes de que se haga una acci贸n u otra d谩ndole mayor porcentaje a la acci贸n m谩s 贸ptima para esa situaci贸n. Tras gran cantidad de pruebas y evaluaciones se han llegado a los porcentajes que se han usado finalmente, ya que ofrecen la experiencia m谩s equilibrada entre dificultad y satisfacci贸n.
Las acciones posibles que puede realizar son: agacharse, levantarse, saltar, atacar hacia arriba, atacar hacia el centro, atacar hacia abajo, protegerse o no hacer nada.
<br><br>
<img src="./Media/EsquemaAcciones.png" height=400 >
<br> <br>

Tras esta toma de decisiones se llama a los m茅todos correspondientes que se encargan de ejecutar las instrucciones dadas, tanto de movimiento como de acci贸n.

<br>
<hr>

## __Partida de demostraci贸n__

<br>

<a href="https://drive.google.com/file/d/1aWZ-EqAb74lG18C64XpNuUffYMOViT5C/view?usp=sharing"><img src="./Media/demoPortada.jpg" height=400></a>

<br>
<hr>

## __Explicaci贸n de los scripts implementados__


- **GameManager** Controla par谩metros comunes a ambos jugadores como el tiempo de la partida y si la partida sigue en juego o ha acabado. Tambi茅n permite que los 鈥渃ontroller鈥? obtengan informaci贸n del contrario pero 煤nicamente de lectura como su posici贸n o la vida. Puede ajustar la orientaci贸n de los personajes en el caso de que se hayan cruzado y determinar qui茅n debe de ganar la partida ya sea porque el tiempo se ha agotado o uno de los contrincantes ha acabado KO.
<br>

- **FightingController:**  Clase de la que heredan tanto PlayerController como EnemyAI la cual cuenta con variables de estado (quieto, saltando, agachado...), m茅todos de interfaz que implementar谩n las clases que derivan de ella, array de sonidos para los movimientos y variables de configuraci贸n.
<br>

- **PlayerController:** Script para que una persona pueda controlar al jugador usando A y D para el movimiento horizontal, W para saltar y S para agacharse. Para atacar se usan J, K y L para ataque alto, medio y bajo respectivamente reserv谩ndose la I para protegernos.
<br>

- **EnemyAI:** Script que controla de forma aut贸noma al enemigo y el cual act煤a seg煤n el estado de la partida y de que est茅 haciendo el jugador tomando decisiones sobre movimiento y acciones a realizar bas谩ndose en un sistema de porcentajes con diferentes posibilidades.
<br>

- **Timer:** Gestiona y dibuja en pantalla con el formato correcto el tiempo que queda de partida permitiendo al GameManager consultarlo para ver si se ha agotado el tiempo.
<br>

- **BarraVida:** Se encarga de gestionar la vida del jugador y de actualizar la interfaz para reflejar el estado. Para dar un mejor aspecto se han a帽adido sistemas de part铆culas para que sea visualmente m谩s evidente cuando pierde vida uno de los luchadores.
<br>

- **AtaqueAcertado:** Script asociado a las hitbox de los ataques que se encargan de comprobar si han colisionado con uno de los luchadores y seg煤n sea uno u otro llama a su m茅todo GestionaDa帽o para que aplique el da帽o oportuno seg煤n el jugador est茅 cubierto o no.

<br>
<hr>

## __Pruebas realizadas__

- **Protegerse si est谩 cerca** [V铆deo demo](https://drive.google.com/file/d/13vxTIOks1LUMHzWYq72TWKWeLswj71vi/view?usp=sharing)

- **Atacar hacia arriba si el contrincante est谩 en el aire** [V铆deo demo](https://drive.google.com/file/d/1h7f621-wncwKS6MxnsdnxjMNnEgV9q5C/view?usp=sharing)

- **Atacar si est谩 en rango** [V铆deo demo](https://drive.google.com/file/d/1Ko1FS8jqBE0LKozJCNXsU3dk0zuD4KDl/view?usp=sharing)

- **Alejarse si est谩 cerca** [V铆deo demo](https://drive.google.com/file/d/1QdXjKKS7iq4At2p-KRYRg_Fqkhtx-fqC/view?usp=sharing)

- **Da帽o reducido si est谩 protegido** [V铆deo demo](https://drive.google.com/file/d/1xiB9V_afiB-i-A-xWYwYKYvwEqjvgdyW/view?usp=sharing)

- **Acercarse si est谩 lejos** [V铆deo demo](https://drive.google.com/file/d/1_zeMSqeRQ_OfG6RwqSUyZlVMh0lcIXjt/view?usp=sharing)

- **Gana si derrota al jugador** [V铆deo demo](https://drive.google.com/file/d/1Qulr-U-E0JLAayR5zXeRMvXCrCV0i1pR/view?usp=sharing)

- **Pierde si el jugador le derrota** [V铆deo demo](https://drive.google.com/file/d/1S-cnyfivoMP3Qc1xQeTEBWSlZ1OuP_xh/view?usp=sharing)

- **Empata si el tiempo se agota y tienen la misma vida** [V铆deo demo](https://drive.google.com/file/d/1m0X1Q45Xz_oi-u1nXqzgyMcAxYSxhSOg/view?usp=sharing)

<br>
<hr>

## __Otros elementos del proyecto__


- Desde el editor de Unity se ofrecen gran cantidad de par谩metros modificables para desarrolladores que permiten cambiar c贸mo se desarrolla la partida y la dificultad de la IA.

- Hemos agregado un **bot贸n para reiniciar** la escena, de tal forma que ayuda a la hora de realizar pruebas de funcionalidad o por si se produjera cualquier error.

- Hemos agregado un **bot贸n para salir** de la build del juego.

- Hemos a帽adido modelos y decorados para dar un mejor aspecto al prototipo y aportarle personalidad propia, adem谩s de servir de feedback visual (por ejemplo: se muestra un escudo al protegerse).

- Las barras de vida son din谩micas y bajan su valor gradualmente dando un aspecto progresivo y no tan seco.

- Hemos a帽adido m煤sica de fondo y sonidos para dar feedback extras a las acciones e interacciones que ocurren en el juego.

<br>
<hr>

## __Lugares de consulta__

- https://medium.com/gyroscopesoftware/how-we-built-an-ai-to-play-street-fighter-ii-can-you-beat-it-9542ba43f02b
- https://sf2platinum.wordpress.com/2017/01/20/the-ai-engine/
- https://intellipaat.com/community/3628/how-to-design-the-artificial-intelligence-of-a-fighting-game-street-fighter-or-soul-cali-bur
- https://www.reddit.com/r/gamemaker/comments/c9khem/fighting_game_ai/
- https://www.reddit.com/r/Skullgirls/comments/82xdf6/skullgirls_ai_teaching_a_machine_to_fight/
- https://forum.unity.com/threads/fighting-game-ai.241597/