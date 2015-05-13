var MultiTitan = MultiTitan ||
    new function(){
        this.scripts = [];

        this.Script =
        function(featureID, featureElement)
        {
            this.featureID = featureID;
            this.featureElement = featureElement;
        };
    };