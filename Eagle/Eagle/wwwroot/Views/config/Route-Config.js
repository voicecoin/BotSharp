import React from 'react'
import {Router, Route,  IndexRoute, Redirect, hashHistory, browserHistory} from 'react-router'
import AppContainer from '../containers/AppContainer'
import LandingContainer from '../containers/LandingPage/LandingPage'
import EmptyAppContainer from '../containers/EmptyAppContainer'
import Http from '../components/XmlHttp';
const http = new Http();
import {DataURL} from './DataURL-Config';
const LoginContainer = (location, cb) => {
  require.ensure([], require => {
    cb(null, require('../containers/LoginPage/LoginPage').default)
  },'LoginContainer')
}

const NewFieldsContainer = (location, cb) => {
  require.ensure([], require => {
    cb(null, require('../containers/UnderlyingStructure/Bundles/NewFieldsContainer').default)
  },'NewFieldsContainer')
}


const FieldsContainer = (location,cb) => {
  require.ensure([], require => {
    cb(null,require('../containers/UnderlyingStructure/Bundles/FieldsContainer').default)
  },'FieldsContainer')
}


const NewBundleContainer = (location, cb) => {
  require.ensure([], require => {
    cb(null, require('../containers/UnderlyingStructure/Bundles/NewBundleContainer').default)
  },'NewBundleContainer')
}


const AddRecordContainer = (location,cb)=>{
    require.ensure([],require=>{
      cb(null,require('../containers/UnderlyingStructure/Bundles/AddRecordContainer').default)
    },'AddRecordContainer')
}

//add router for record history container
const RecordContainer = (location,cb) => {
    require.ensure([],require => {
      cb(null,require('../containers/UnderlyingStructure/Records/RecordContainer').default)
    },'RecordContainer')
}


//add router for TaxonomyContainer
const TaxonomyContainer = (location,cb) => {
    require.ensure([],require => {
      cb(null,require('../containers/Vocabulary/Taxonomies/TaxonomyContainer').default)
    },'TaxonomyContainer')
}

//add a router for TaxonomyTermContainer
const TaxonomyTermContainer = (location,cb) => {
    require.ensure([],require => {
      cb(null,require('../containers/Vocabulary/Taxonomies/TaxonomyTermContainer').default)
    },'TaxonomyTermContainer')
}

//add a router for RulesContainer
const RulesContainer = (location,cb) => {
    require.ensure([],require => {
      cb(null,require('../containers/UnderlyingStructure/Rules/RulesContainer').default)
    },'RulesContainer')
}

//add a router for ViewsContainer
const ViewsContainer = (location,cb) => {
    require.ensure([],require => {
      cb(null,require('../containers/UnderlyingStructure/Views/ViewsContainer').default)
    },'ViewsContainer')
}

//add a router for RulesConditionContainer
const RulesConditionContainer = (location,cb) => {
    require.ensure([],require => {
      cb(null,require('../containers/UnderlyingStructure/Rules/RulesConditionContainer').default)
    },'RulesConditionContainer')
}

//add a router for NewRulesContainer
const NewRulesContainer = (location,cb) => {
    require.ensure([],require => {
      cb(null,require('../containers/UnderlyingStructure/Rules/NewRulesContainer').default)
    },'NewRulesContainer')
}

//add a router for Home Page
const HomeContainer = (location,cb) => {
    require.ensure([],require => {
      cb(null,require('../containers/UnderlyingStructure/Bundles/HomeContainer').default)
    },'HomeContainer')
}

//add a router for Register Page
const RegisterContainer = (location,cb) => {
    require.ensure([],require => {
      cb(null,require('../containers/RegisterPage/RegisterPage').default)
    },'RegisterContainer')
}

//add a router for Profile Page
const ProfileContainer = (location,cb) => {
    require.ensure([],require => {
      cb(null,require('../containers/ProfilePage/ProfilePage').default)
    },'ProfileContainer')
}

//add a router for Dash Board Page
const DashBoardContainer = (location,cb) => {
    require.ensure([],require => {
      cb(null,require('../containers/DashBoard/DashBoard').default)
    },'DashBoardContainer')
}

//add a router for getting back password Page
const ForgetPasswordContainer = (location,cb) => {
    require.ensure([],require => {
      cb(null,require('../containers/ForgetPassword/ForgetPassword').default)
    },'ForgetPasswordContainer')
}

//add a router for shared Page
const SharedContainer = (location,cb) => {
    require.ensure([],require => {
      cb(null,require('../containers/SharedContainer/SharedContainer').default)
    },'SharedContainer')
}

//add a router for settings Page
const Settings = (location,cb) => {
    require.ensure([],require => {
      cb(null,require('../containers/Settings/Settings').default)
    },'Settings')
}

//add a router for design layout Page
const PageLayout = (location,cb) => {
    require.ensure([],require => {
      cb(null,require('../containers/PageLayout/PageLayout').default)
    },'PageLayout')
}

//add a router for createa layout Page
const NewPage = (location,cb) => {
    require.ensure([],require => {
      cb(null,require('../containers/PageLayout/NewPage').default)
    },'NewPage')
}

function requireAuth(nextState, replace, callback){
  if(localStorage.getItem('access_token')){
    http.HttpAjax({
        url: DataURL + '/api/Bundle',
        headers: {'authorization':'Bearer ' + localStorage.getItem('access_token')}
      }).then((data) => {
        callback();
      })
        .catch((e)=>{
          replace({pathname:'/'});
          callback();
        })
  }
  else{
    replace({pathname:'/'});
    callback();
  }

}

function alreadyLogin(nextState, replace, callback){
  if(localStorage.getItem('access_token')){
    http.HttpAjax({
        url: DataURL + '/api/Bundle',
        headers: {'authorization':'Bearer ' + localStorage.getItem('access_token')}
      }).then((data) => {
        replace({pathname:'/Structure/Bundles'});
        callback();
      })
        .catch((e)=>{
          callback();
        })
  }
  else callback();
}

const RootRoter = (
  <Router history={ hashHistory }>
    <Route path='/Structure' component={ AppContainer } onEnter={requireAuth}>
      <Route path="Record" getComponent={RecordContainer}/>
      <Route path="Taxonomy" getComponent={TaxonomyContainer}/>
      <Route path='Taxonomy'>
        <Route path="TaxonomyTerm" getComponent={TaxonomyTermContainer}/>
      </Route>
      <Route path="Rules" getComponent={RulesContainer}/>
      <Route path='Rules'>
        <Route path="RulesCondition" getComponent={RulesConditionContainer}/>
        <Route path="NewRules" getComponent={NewRulesContainer}/>
      </Route>
      <Route path="Views" getComponent={ViewsContainer}/>
      <Route path="Pages" getComponent={NewPage}/>
      <Route path='Pages'>
        <Route path="PageLayout" getComponent={PageLayout}/>
      </Route>
      <Route path="Bundles" getComponent={HomeContainer} />
      <Route path='Bundles'>
        <Route path="fields" getComponent={FieldsContainer}/>
        <Route path="NewFields" getComponent={NewFieldsContainer} />
        <Route path="NewBundle" getComponent={NewBundleContainer} />
        <Route path="AddRecord" getComponent={AddRecordContainer}/>
      </Route>
    </Route>
    <Route component={ AppContainer } onEnter={requireAuth}>
      <Route path='/Settings' getComponent={ Settings }/>
    </Route>
    <Route path='/Shared' component={ AppContainer } onEnter={requireAuth}>
      <Route path='Page/:value' getComponent={ SharedContainer }/>
    </Route>
    <Route path='/Configuration' component={ AppContainer } onEnter={requireAuth}>
      <Route path='People'>
        <Route path="Profile" getComponent={ProfileContainer}/>
      </Route>
      <Route path='Development'>
        <Route path="Profile" getComponent={ProfileContainer}/>
      </Route>
    </Route>
    <Route component={AppContainer} onEnter={requireAuth}>
      <Route path="Dashboard" getComponent={DashBoardContainer}/>
    </Route>
    <Route path="/" component={EmptyAppContainer} onEnter={alreadyLogin}>
      <IndexRoute component={ LandingContainer }/>
      <Route path="Register" getComponent={RegisterContainer}/>
      <Route path="Login" getComponent={LoginContainer}/>
      <Route path="ForgetPassword" getComponent={ForgetPasswordContainer}/>
    </Route>
  </Router>
)
export default RootRoter
