const routers = [
    {
        path: '/',
        meta: {
            title: 'YAYABOT'
        },
        component: (resolve) => require(['./views/index.vue'], resolve)
    },
	{
        path: '/dashboard',
        meta: {
            title: 'Dashboard'
        },
        component: (resolve) => require(['./views/master.vue'], resolve)
    },
	{
        path: '/users',
        meta: {
            title: 'Users'
        },
        component: (resolve) => require(['./views/master.vue'], resolve)
    },
	{
        path: '/agents',
        meta: {
            title: 'Agents'
        },
        component: (resolve) => require(['./views/master.vue'], resolve)
    }
];
export default routers;