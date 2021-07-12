var passwordList = [];
function hasClass(el, className) {
    if (el.classList) {

        return el.classList.contains(className);
    }
    else {
        new RegExp('(^| )' + className + '( |$)', 'gi').test(el.className);
    }
}
function Register() {

    var helperTextr = {
        charLength: document.querySelector('.helper-text .length'),
        lowercase: document.querySelector('.helper-text .lowercase'),
        uppercase: document.querySelector('.helper-text .uppercase'),
        digit: document.querySelector('.helper-text .digit'),
        special: document.querySelector('.helper-text .special')

    };
    if (hasClass(helperTextr.charLength, 'valid') &&
        hasClass(helperTextr.lowercase, 'valid') &&
        hasClass(helperTextr.uppercase, 'valid') &&
        hasClass(helperTextr.special, 'valid') &&
        hasClass(helperTextr.digit, 'valid')

    ) {
       

        $.post('/home/register', $('form').serialize(), function (data) {
            if (data == 1) {
                alert('Save successfull');
                window.location = "/home"
            }
                
        });

    }

}


function Login() {

   
    if (!!$("#userName").val() && !!$("#userName").val()) {
       
        $.get('/home/login', $('form').serialize(), function (data) {          
            if (data == "1")
                window.location = "/home/changePassword"
        });

    }

}
function ChangePass() {

    var helperTextr = {
        charLength: document.querySelector('.helper-text .length'),
        lowercase: document.querySelector('.helper-text .lowercase'),
        uppercase: document.querySelector('.helper-text .uppercase'),
        digit: document.querySelector('.helper-text .digit'),
        special: document.querySelector('.helper-text .special'),
        match: document.querySelector('.helper-text .match')
    };

    if (hasClass(helperTextr.charLength, 'valid') &&
        hasClass(helperTextr.lowercase, 'valid') &&
        hasClass(helperTextr.uppercase, 'valid') &&
        hasClass(helperTextr.special, 'valid') &&
        hasClass(helperTextr.digit, 'valid') &&
        hasClass(helperTextr.match, 'valid')

    ) {
             
        $.post('/home/changePassword', $('form').serialize(), function (data) {

            alert(data);
        });

    }

}

function OnLoad () {
        var password = document.querySelector('.password');
    
        var email = document.querySelector('.email');

        var helperText = {
            charLength: document.querySelector('.helper-text .length'),
            lowercase: document.querySelector('.helper-text .lowercase'),
            uppercase: document.querySelector('.helper-text .uppercase'),
            special: document.querySelector('.helper-text .special'),
            validEmail: document.querySelector('.helper-text .email'),
            digit: document.querySelector('.helper-text .digit'),
            match: document.querySelector('.helper-text .match')
        };

    var pattern = {
       
            charLength: function () {
                if (password.value.length >= 10) {
                    return true;
                }
            },
            match: function () {
               
                if ($("#newpassword").val() != "" && $("#confirmPassword").val() != "" && $("#newpassword").val() == $("#confirmPassword").val()) {
                    
                    return true;
                }
            },
            
            digit: function () {
                ///[0-9]/g
               // var regex = (/^\d{1,2}$/);
                var regex = (/\d/);
                if (regex.test(password.value)) {
                    return true;
                }
            },


            lowercase: function () {
                var regex = (/(?:[^a-z]*[a-z]){2}/);
                // var regex = (/^(?=.*[a-z]).+$/); // Lowercase character pattern

                var LowerCase = regex.test(password.value);
                var countLower = (password.value).length >= 2;
                if (LowerCase) {
                    console.log("Checks for lowercase ", LowerCase);

                    if (countLower) {
                        console.log("Checks for 2 lowercases" + countLower);

                        return true;
                    }
                }


            },
            uppercase: function () {
                var regex = /^(?=.*[A-Z]).+$/; // Uppercase character pattern

                if (regex.test(password.value)) {
                    return true;
                }
            },
            validEmail: function () {
                var regex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
               // var regex = /^([\w-\.]+)@@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$/; // Email character pattern
                
                if (regex.test(email.value)) {
                    return true;
                }
            },
            special: function () {
                //
               // var regex = /^[^a-zA-Z0-9\-\/].+$/;
               var regex = /^(?=.*[_\W]).+$/; // Special character or number pattern

                if (regex.test(password.value)) {
                    return true;
                }
            }
        };
    
        email.addEventListener('keyup', function () {

            // Check valid email
            patternTest(pattern.validEmail(), helperText.validEmail);
        });

        // Listen for keyup action on password field
    password.addEventListener('keyup',
        OnKeyUP()
    );
    $("#confirmPassword").keyup(OnKeyUP);
   
    function OnKeyUP() {
        // Check that password is a minimum of 10 characters
        patternTest(pattern.charLength(), helperText.charLength);

        // Check that password contains a lowercase letter
        patternTest(pattern.lowercase(), helperText.lowercase);

        // Check that password contains an uppercase letter
        patternTest(pattern.uppercase(), helperText.uppercase);

        // Check that password contains an digit
        patternTest(pattern.digit(), helperText.digit);

        // Check that password contains a number or special character
        patternTest(pattern.special(), helperText.special);

        // Check that password contains a number or special character
        patternTest(pattern.match(), helperText.match);

        // Check that all requirements are fulfilled
        if (hasClass(helperText.charLength, 'valid') &&
            hasClass(helperText.lowercase, 'valid') &&
            hasClass(helperText.uppercase, 'valid') &&
            hasClass(helperText.special, 'valid') &&
            hasClass(helperText.digit, 'valid') &&
            hasClass(helperText.match, 'valid')

        ) {
            addClass(password.parentElement, 'valid');
            var btn = document.getElementById("btnSave");
            btn.disabled = false;
        }
        else {
            removeClass(password.parentElement, 'valid');
        }
    }
        function patternTest(pattern, response) {
            if (pattern) {
                addClass(response, 'valid');
            }
            else {
                removeClass(response, 'valid');
                //addClass(response, 'invalid');
            }
        }

        function addClass(el, className) {
            if (el.classList) {
                el.classList.add(className);
            }
            else {
                el.className += ' ' + className;
            }
        }

        function removeClass(el, className) {
            if (el.classList)
                el.classList.remove(className);
            else
                el.className = el.className.replace(new RegExp('(^|\\b)' + className.split(' ').join('|') + '(\\b|$)', 'gi'), ' ');
        }



    };
function OnKeyUpLogin() {
    if (!!!$("#userName").val() || !!!$("#userName").val()) {
        alert("Please fill out all fields");
    } else {
       var btn = document.getElementById("btnLogin");
       btn.disabled = false;
    }
}