Watson
===================
Watson is a machine learning IRC automated system. He is a project designed to utilize most languages, and so far his implementation has been completed in the following languages;

 - Java (click [here](https://github.com/krich38/Watson-Java))
 - C# (click [here](https://github.com/krich38/Watson-CSharp))

**Features**

 1. User Control
 2. Markov Machine Learning
 3. User login
 4. Configuration loading and saving, easy customization
 5. User commands

User Control
===================
To control Watson, you must have a recognized host name, or have a log in account. If your host name is not recognized, you can regain control of Watson by private messaging him and logging in. A test account with the login information is **watsoncontroller**/**password123**

    <krich38> Watson login watsoncontroller password123
    <Watson> Thank you for authenticating, you have been granted FULL_USER access.

Markov Machine Learning
===================
By feeding Watson information, he distinguish and learn new information. He makes use of the Markov machine learning implementation. 
    
By feeding him information, he can learn and adapt, we see when he is fed information below;

    <krich38> Watson, chat about Java
    <Watson> I can't :(
 And by feeding him data; 

    <krich38> Java is the language
    <krich38> Watson is written in Java
    <krich38> Java is Watson
    **....etc...**
He can construct basic sentences when fed enough data;
    
    <krich38> Watson chat about Java
    <Watson> Java is the language Watson is written in

![Brief diagram of the Markov Machine Learning algorithms](https://upload.wikimedia.org/wikipedia/commons/thumb/2/2e/HiddenMarkovModel.png/300px-HiddenMarkovModel.png)
*"A hidden Markov model (HMM) is a statistical Markov model in which the system being modeled is assumed to be a Markov process with unobserved (hidden) states. A HMM can be presented as the simplest dynamic Bayesian network. The mathematics behind the HMM was developed by L. E. Baum and coworkers. It is closely related to an earlier work on the optimal nonlinear filtering problem by Ruslan L. Stratonovich, who was the first to describe the forward-backward procedure.
In simpler Markov models (like a Markov chain), the state is directly visible to the observer, and therefore the state transition probabilities are the only parameters. In a hidden Markov model, the state is not directly visible, but output, dependent on the state, is visible. Each state has a probability distribution over the possible output tokens. Therefore the sequence of tokens generated by an HMM gives some information about the sequence of states. Note that the adjective 'hidden' refers to the state sequence through which the model passes, not to the parameters of the model; the model is still referred to as a 'hidden' Markov model even if these parameters are known exactly."* -  Wikipedia

User Commands
===================
Watson has a few user controllable commands. A few are below, with examples;

    <krich38> Watson, say Hello World!
    <Watson> Hello World!

    <krich38> Watson, weather Newcastle Au
    <Watson> Yz85Racer: Weather for Newcastle, Australia, Sky is Clear, Temp 19.32c (min 19.32c/max 19.32c), 92% Humidity, 1032.47 hPa, 0% Cloudy, Wind Speed 3.91m/s

    <krich38> Watson, urban Java
    <Watson> Yz85Racer: Urban definition of `Java`: A programming language commonly used as a solution to everything and anything.